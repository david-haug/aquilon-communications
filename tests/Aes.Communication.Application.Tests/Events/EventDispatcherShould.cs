using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Events;
using Aes.Communication.Tests.Common.Fakes.Events;
using Xunit;

namespace Aes.Communication.Application.Tests.Events
{
    public class EventDispatcherShould
    {
        //[Fact]
        //public void ClearEventsAfterDispatching()
        //{
        //    var raiser = new MockDomainEventRaiser();
        //    raiser.Events.Add(new MockDomainEvent());
        //    var sut = new EventDispatcher(new StubEventRepository());
        //    sut.Dispatch(raiser);

        //    Assert.Empty(raiser.Events);

        //}
    }
}
