using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Tests.Common.Fakes.Messages
{
    public class StubMessageRepository: IMessageRepository
    {
        public Message Get(Guid id)
        {
            return null;
        }

        public void Add(Message message)
        {
            //do nothing
        }

        public void Update(Message message)
        {
            //do nothing
        }
    }
}
