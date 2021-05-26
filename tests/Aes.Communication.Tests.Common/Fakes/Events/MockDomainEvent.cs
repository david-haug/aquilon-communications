using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain;

namespace Aes.Communication.Tests.Common.Fakes.Events
{
    public class MockDomainEvent: IDomainEvent
    {
        public IEntity Entity { get; set; }
        public DateTimeOffset DateOccurred { get; set; }
    }
}
