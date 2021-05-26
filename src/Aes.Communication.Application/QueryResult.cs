using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Application
{
    public class QueryResult<T>
    {
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Total number of records found by query
        /// </summary>
        public int TotalCount { get; set; }
    }
}
