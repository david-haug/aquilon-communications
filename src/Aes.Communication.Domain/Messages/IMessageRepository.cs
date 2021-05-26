using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.Messages
{
    public interface IMessageRepository
    {
        Message Get(Guid id);
    }
}
