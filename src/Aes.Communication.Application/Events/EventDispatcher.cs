using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Domain;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Application.Events
{
    public class EventDispatcher: IEventDispatcher
    {
        private IEventRepository _repository;

        public EventDispatcher(IEventRepository repository)
        {
            _repository = repository;
        }

        ////hack...just handling saving here for now
        //public void Dispatch(IDomainEventRaiser eventRaiser)
        //{
        //    foreach(var e in eventRaiser.Events)
        //        _repository.Save(new Event(e.DateOccurred,e.GetType().Name,e.Entity));

        //    eventRaiser.Events.Clear();
        //}

        //public void Dispatch(IEnumerable<IDomainEvent> domainEvents)
        //{
        //    foreach (var e in domainEvents)
        //        _repository.Save(new Event(e.DateOccurred, e.GetType().Name, e.Entity));


        //}


        public void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IDomainEvent
        {
            _repository.Save(new Event(eventToDispatch.DateOccurred, eventToDispatch.GetType().Name, eventToDispatch.Entity));
        }
    }
}
