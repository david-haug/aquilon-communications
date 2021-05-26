using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Events;

namespace Aes.Communication.Tests.Common.Fakes.Events
{
    public class StubEventRepository : IEventRepository
    {
        public void Save(Event @event)
        {
            //do nothing
        }

        public IEnumerable<Event> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
