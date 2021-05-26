using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Application.Events
{
    public class Event
    {
        public Event(DateTimeOffset date, string name, object content)
        {
            Id = Guid.NewGuid();
            DateOccurred = date;
            Name = name;
            Content = content;
        }

        public Guid Id { get; set; }
        public DateTimeOffset DateOccurred { get; set; }
        public string Name { get; set; }
        public object Content { get; set; }
    }
}
