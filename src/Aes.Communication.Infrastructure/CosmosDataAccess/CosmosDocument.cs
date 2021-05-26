using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Infrastructure.CosmosDataAccess
{
    public class CosmosDocument<T>
    {
        public Guid Id { get; set; }
        public string PartitionKey { get; set; }
        public T Item { get; set; }
    }
}
