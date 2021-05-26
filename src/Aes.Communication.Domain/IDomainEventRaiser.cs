using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain
{
    public interface IDomainEventRaiser
    {
        ICollection<IDomainEvent> Events { get; }
    }
}
