using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aes.Communication.Api.Models
{
    /// <summary>
    /// Model class for a conversation topic.  Existing header will be overwritten with values of this object.
    /// </summary>
    public class TopicModel
    {
        /// <summary>
        /// Required title of the conversation
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Key-value pairs for optional topic attributes specific to this conversation (eg - "InvoiceId": "ABC12345")
        /// </summary>
        public Dictionary<string, string> Attributes { get; set; }
    }
}
