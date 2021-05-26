using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aes.Communication.Infrastructure.CosmosDataAccess.Repositories
{
    public class ConversationRepository: IConversationRepository
    {
        private string _database;
        private string _collection;
        private CosmosConnectionString _connection;

        public ConversationRepository(string connection, string databaseName, string collectionName)
        {
            _connection = new CosmosConnectionString(connection);
            _database = databaseName;
            _collection = collectionName;
        }

        public ConversationRepository(CosmosConnectionString cosmosConnection, string databaseName, string collectionName)
        {
            _connection = cosmosConnection;
            _database = databaseName;
            _collection = collectionName;
        }

        public IEnumerable<Conversation> GetAll()
        {
            throw new NotImplementedException();
        }

        public Conversation Get(Guid id)
        {
            var sql = $"select * from c where c.id = \"{id.ToString()}\"";
            return GetBySql(sql);
        }

        public Conversation GetBySubject(EntityId entityId)
        {
            var sql = $"select * from c where c.item.subject.id = \"{entityId.Id}\" and c.item.subject.type = \"{entityId.Type}\"";
            return GetBySql(sql);
        }

        private Conversation GetBySql(string sql)
        {
            var client = new DocumentClient(_connection.Endpoint, _connection.Key, new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
            }, ConsistencyLevel.Eventual,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            using (client)
            {
                var link = UriFactory.CreateDocumentCollectionUri(_database, _collection);
                var document = client.CreateDocumentQuery<CosmosDocument<Entities.Conversation>>(link, sql, new FeedOptions
                {
                    EnableCrossPartitionQuery = true
                }).AsEnumerable().FirstOrDefault();

                var dto = document?.Item;
                if (dto == null) return null;

                List<Message> messages = null;
                if (dto.Messages != null)
                {
                    messages = new List<Message>();
                    foreach (var m in dto.Messages)
                    {
                        var message = Message.Load(m.Id, m.ConversationId, new EntityId(m.Subject.Id, (MessageEntityType)m.Subject.EntityType), m.Body, m.IsPublic,
                            new Domain.ValueObjects.User
                            {
                                UserId = m.User.UserId,
                                FirstName = m.User.FirstName,
                                LastName = m.User.LastName,
                                Email = m.User.Email
                            },
                            m.DateCreated,
                            m.ReadByUsers,
                            m.Organization);

                        if (m.Attachments != null)
                            message.Attachments = m.Attachments;

                        messages.Add(message);
                    }
                }

                EntityId parent = null;
                if (dto.Parent != null)
                    parent = new EntityId(dto.Parent.Id, (MessageEntityType)dto.Parent.EntityType);

                return Conversation.Load(dto.Id,
                    new EntityId(dto.Subject.Id, (MessageEntityType)dto.Subject.EntityType),
                    dto.DateCreated,
                    dto.CreatedByUserId,
                    messages,
                    new ConversationTopic(dto.Topic.Title, dto.Topic.Attributes), null,
                    parent,
                    dto.OrganizationId,
                    dto.CounterpartyId);
            }
        }

        public async Task Save(Conversation conversation)
        {
            var client = new DocumentClient(_connection.Endpoint, _connection.Key, serializerSettings: new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            var document = await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(_database, _collection), 
                new CosmosDocument<Conversation>
                {
                    Item = conversation,
                    PartitionKey = $"{conversation.OrganizationId}:{conversation.CounterpartyId}",
                    Id = conversation.Id
                });

        }
    }
}
