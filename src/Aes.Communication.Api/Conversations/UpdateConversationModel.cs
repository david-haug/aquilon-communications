using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Models;

namespace Aes.Communication.Api.Conversations
{
    /// <summary>
    /// Request object to update the conversation's topic and/or parent.
    /// </summary>
    public class UpdateConversationModel
    {
        /// <summary>
        /// Object representing the conversation's title and metadata (attributes). Will not be updated if not supplied.
        /// </summary>
        public TopicModel Topic { get; set; }
        /// <summary>
        /// Business entity, usually a tieout, used to group related conversations. Will not be updated if not supplied.
        /// </summary>
        public EntityIdModel Parent { get; set; }
    }
}
