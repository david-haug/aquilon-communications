using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Application.Conversations
{
    public class ConversationNavigationItemDto
    {
        public string ConversationId { get; set; }
        public string Topic { get; set; }  
        public string TopicAttributes { get; set; }
        public int UnreadMessageCount { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
