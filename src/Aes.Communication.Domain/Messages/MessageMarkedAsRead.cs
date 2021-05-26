using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.Messages
{
    public class MessageMarkedAsRead : IDomainEvent
    {
        public MessageMarkedAsRead()
        {

        }
        public MessageMarkedAsRead(Message message, DateTimeOffset dateOccurred)
        {
            Entity = message;
            DateOccurred = dateOccurred;
        }
        public IEntity Entity { get; set; }
        public DateTimeOffset DateOccurred { get; set; }
    }
}
