using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.Conversations
{
    public class ConversationUserFlagRemoved : IDomainEvent
    {
        public ConversationUserFlagRemoved(ConversationUserFlag flag, DateTimeOffset dateOccurred)
        {
            Entity = flag;
            DateOccurred = dateOccurred;
        }
        public IEntity Entity { get; set; }
        public DateTimeOffset DateOccurred { get; set; }

    }
}
