using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Events;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Infrastructure.Json;

namespace Aes.Communication.Infrastructure.Events
{
    public class EventJsonRepository: JsonRepository<Event>, IEventRepository
    {
        private string _file;
        public EventJsonRepository(string file)
        {
            _file = file;
        }

        public void Save(Event @event)
        {
            var data = Create(_file, new EventParser());
            data.Add(@event);
            SaveJsonToFile(data, _file);
        }

        public IEnumerable<Event> GetAll()
        {
            var data = Create(_file, new EventParser());
            return data;
        }
    }

    public class EventParser : IDomainObjectParser<Event>
    {
        public Event Create(dynamic e)
        {
            return new Event((DateTimeOffset)e.dateOccurred,e.name.ToString(), (object)e.content);
        }
    }
}
