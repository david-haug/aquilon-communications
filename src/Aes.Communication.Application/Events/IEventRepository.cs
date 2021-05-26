using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Application.Events
{
    public interface IEventRepository
    {
        void Save(Event @event);
        IEnumerable<Event> GetAll();
    }
}
