using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.GetConversations;
using Aes.Communication.Application.Conversations.Repositories;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aes.Communication.Infrastructure.CosmosDataAccess.Repositories
{
    public class ConversationReadOnlyRepository: IConversationReadOnlyRepository
    {
        private DocumentClient _client;
        private CosmosConnectionString _connection;
        private string _database;
        private string _collection;

        public ConversationReadOnlyRepository(string connection, string databaseName, string collectionName)
        {
            _database = databaseName;
            _collection = collectionName;
            _connection = new CosmosConnectionString(connection);
            _client = new DocumentClient(_connection.Endpoint, _connection.Key, new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp,
                }, ConsistencyLevel.Eventual,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }

        public ConversationDto GetConversation(string id)
        {
            var valid = Guid.TryParse(id, out var guid);
            if (!valid) return null;

            var conversationRepository = new ConversationRepository(_connection, _database, _collection);
            var conversation = conversationRepository.Get(guid);
            return ConversationDto.Map(conversation);
        }

        public IEnumerable<ConversationDto> GetAllConversations()
        {
            using (_client)
            {
                var link = UriFactory.CreateDocumentCollectionUri(_database, _collection);
                var sql = $"select * from c";
                var docs = _client.CreateDocumentQuery<ConversationDto>(link, sql, new FeedOptions
                {
                    EnableCrossPartitionQuery = true,
                    MaxItemCount = 100
                }).ToList();

                return docs;
            }
        }

        public IEnumerable<ConversationDto> GetConversations(GetConversationsRequest query)
        {
            //using (_client)
            //{
            //    var link = UriFactory.CreateDocumentCollectionUri(_database, _collection);
            //    var docs = _client.CreateDocumentQuery<CosmosDocument<ConversationDto>>(link, CreateSqlStatement(query), new FeedOptions
            //    {
            //        EnableCrossPartitionQuery = true,
            //        MaxItemCount = -1
            //    }).ToList();

            //    return docs.Select(d=>d.Item).ToList();
            //}

            using (_client)
            {
                var link = UriFactory.CreateDocumentCollectionUri(_database, _collection);
                var docs = _client.CreateDocumentQuery<CosmosDocument<Entities.Conversation>>(link, CreateSqlStatement(query), new FeedOptions
                {
                    EnableCrossPartitionQuery = true,
                    MaxItemCount = -1
                }).ToList();

                var conversations = new List<ConversationDto>();
                var convDocs = docs.Select(d => d.Item).ToList();
                foreach (var conv in convDocs)
                {
                    var conversation = ConversationDto.Map(CreateConversation(conv));
                    conversations.Add(conversation);
                }

                return conversations;
            }
        }

        private Conversation CreateConversation(Entities.Conversation dto)
        {
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



        public async Task<IEnumerable<ConversationDto>> GetConversationsAsync(GetConversationsRequest request)
        {

            throw new NotImplementedException();

            ////SELECT * FROM c where c.subject.id="1" and c.subject.type="invoice"
            //var client = Context.CreateDocumentClient();
            //IDocumentQuery<ConversationDto> query = client.CreateDocumentQuery<ConversationDto>(
            //        UriFactory.CreateDocumentCollectionUri("Communications", "Conversations"),
            //        new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
            //    .Where(c=> request.Parent!=null ? (request.Parent.Id == c.Parent.Id && request.Parent.Type == c.Parent.Type) : null)
            //    .AsDocumentQuery();

            //List<ConversationDto> results = new List<ConversationDto>();
            //while (query.HasMoreResults)
            //{
            //    results.AddRange(await query.ExecuteNextAsync<ConversationDto>());
            //}

            //return results;
        }

        public string CreateSqlStatement(GetConversationsRequest request)
        {
            var sql = $"select * from c where ";
            var parentFilter = "";
            if (request.Parent != null)
                parentFilter = $"c.item.parent.type = '{request.Parent.Type}' and c.item.parent.id = '{request.Parent.Id}' and ";

            var subjectFilter = "";
            if (request.Subject != null)
            {
                subjectFilter = $"c.item.subject.type = '{request.Subject.Type}' and ";

                var idFilter = "";
                if (!string.IsNullOrWhiteSpace(request.Subject.Id))
                    idFilter = $"c.item.subject.id = '{request.Subject.Id}' and ";

                subjectFilter += idFilter;
            }

            sql += parentFilter + subjectFilter;
            if (sql.EndsWith("and "))
                sql = sql.Remove(sql.Length - 4, 4).Trim();
            if (sql.EndsWith("where "))
                sql = sql.Remove(sql.Length - 6, 6).Trim();
            return sql;
        }


    }
}
