using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Infrastructure.Messages
{
    public class MessageRepository:IMessageRepository
    {
        public Message Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Add(Message message)
        {
            throw new NotImplementedException();
        }

        public void Update(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
