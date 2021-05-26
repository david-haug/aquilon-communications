using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain;

namespace Aes.Communication.Domain.Conversations
{
    public class ConversationCreated : IDomainEvent
    {
        public ConversationCreated(Conversation conversation, DateTimeOffset dateTimeOffset)
        {
            Entity = conversation;
            DateOccurred = dateTimeOffset;
        }
        public IEntity Entity { get; set; }
        public DateTimeOffset DateOccurred { get; set; }
    }
}
