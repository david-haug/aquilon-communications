using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Aes.Communication.Infrastructure.CosmosDataAccess
{
    public class CosmosConnectionString
    {
        public CosmosConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            if (builder.TryGetValue("AccountKey", out var key))
            {
                Key = key.ToString();
            }

            if (builder.TryGetValue("AccountEndpoint", out var uri))
            {
                Endpoint = new Uri(uri.ToString());
            }
        }

        public Uri Endpoint { get; set; }

        public string Key { get; set; }
    }
}