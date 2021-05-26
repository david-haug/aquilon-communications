using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Infrastructure.CosmosDataAccess;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aes.Communication.Api.Logging
{
    public class CosmosLogRepository:ILogRepository
    {
        private string _database;
        private string _collection;
        private CosmosConnectionString _connection;

        public CosmosLogRepository(string connection, string databaseName, string collectionName)
        {
            _connection = new CosmosConnectionString(connection);
            _database = databaseName;
            _collection = collectionName;
        }

        public async Task Save(LogEntry logEntry)
        {
            var client = new DocumentClient(_connection.Endpoint, _connection.Key, serializerSettings: new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_database, _collection),logEntry);
        }
    }
}
