using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Domain.Conversations
{
    public class ConversationMessageAdded: IDomainEvent
    {
        public ConversationMessageAdded(Message message, DateTimeOffset dateOccurred)
        {
            Entity = message;
            DateOccurred = dateOccurred;
        }
        public IEntity Entity { get; set; }
        public DateTimeOffset DateOccurred { get; set; }
    }
}
