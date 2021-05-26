using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Domain.Conversations
{
    public class ConversationParentChanged: IDomainEvent
    {
        public ConversationParentChanged(Conversation conversation, DateTimeOffset dateOccurred)
        {
            Entity = conversation;
            DateOccurred = dateOccurred;

        }
        public IEntity Entity { get; set; }
        public DateTimeOffset DateOccurred { get; set; }
    }
}
