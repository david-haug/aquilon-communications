using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain
{
    public interface IEventDispatcher
    {
        void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IDomainEvent;
    }
}
