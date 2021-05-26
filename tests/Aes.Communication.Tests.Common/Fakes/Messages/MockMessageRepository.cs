using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Tests.Common.Fakes.Messages
{
    public class MockMessageRepository:IMessageRepository
    {
        private Message _message = new Message();
        public Message Get(Guid id)
        {
            if (id.ToString().StartsWith("0000"))
                return null;

            return _message;
        }

        public void Add(Message message)
        {
            _message = message;
        }

        public void Update(Message message)
        {
            _message = message;
        }
    }
}
