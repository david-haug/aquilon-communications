using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain
{
    public static class DomainEvents
    {
        //Raises the given domain event
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            //Dispatcher.Dispatch(args);
            Events.Add(args);
        }

        public static IEventDispatcher Dispatcher { get; set; }

        //how will static 
        public static List<IDomainEvent> Events = new List<IDomainEvent>();

        public static void Dispatch()
        {
            var eventsToDispatch = new List<IDomainEvent>();
            eventsToDispatch.AddRange(Events);
            Events.Clear();

            foreach (var e in eventsToDispatch)
                Dispatcher.Dispatch(e);
        }
    }
}
