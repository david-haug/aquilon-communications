using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Application.Events;
using Aes.Communication.Infrastructure.Events;
using Xunit;

namespace Aes.Communication.Infrastructure.Tests.Events
{
    public class EventJsonRepositoryShould
    {
        private string _file = Settings.Json.EventsFilePath;

        [Fact]
        public void SaveEvent()
        {
            var sut = new EventJsonRepository(_file);
            var @event = new Event(new DateTimeOffset(DateTime.Now), "TestEventHappened", new {value = ":)"});

            var expected = sut.GetAll().Count() + 1;
            sut.Save(@event);
            var actual = sut.GetAll().Count();
            Assert.Equal(expected, actual);
        }
    }
}
