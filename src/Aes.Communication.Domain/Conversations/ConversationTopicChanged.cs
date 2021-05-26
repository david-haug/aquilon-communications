using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.Conversations
{
    public class ConversationTopicUpdated : IDomainEvent
    {
        public ConversationTopicUpdated(ConversationTopic topic, DateTimeOffset dateOccurred)
        {
            Entity = topic;
            DateOccurred = dateOccurred;
        }

        public IEntity Entity { get; set; }
        public DateTimeOffset DateOccurred { get; set; }
    }
}
