using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain
{
    public interface IDomainEvent
    {
        IEntity Entity { get; set; }
        DateTimeOffset DateOccurred { get; set; }
    }
}
