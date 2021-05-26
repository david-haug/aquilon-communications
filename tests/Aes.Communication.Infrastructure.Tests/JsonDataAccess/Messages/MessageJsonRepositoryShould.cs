using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Infrastructure.Messages;
using Aes.Communication.Tests.Common.Fakes.Users;
using Xunit;
namespace Aes.Communication.Infrastructure.Tests.Messages
{
    public class MessageJsonRepositoryShould
    {
        private string _file = Settings.Json.MessagesFilePath;

        [Fact]
        public void AddMessage()
        {
            var message = Message.Load(Guid.NewGuid(), Guid.NewGuid(), new EntityId("1", MessageEntityType.Invoice),"body",true,new UserFactory().Create(),DateTime.Now, null);
            var sut = new MessageJsonRepository(_file);

            sut.Add(message);

            var msg = sut.Get(message.Id);
            Assert.Equal(message.Id, msg.Id);
        }

        [Fact]
        public void GetMessageGivenValidId()
        {
            var id = Guid.Parse("d1aedb65-7060-4ec8-b967-cf888429a86f");
            var sut = new MessageJsonRepository(_file);
            var actual = sut.Get(id);
            Assert.NotNull(actual);

        }

        [Fact]
        public void ReturnNullWhenNotFound()
        {
            var id = Guid.Parse("6636d898-8683-46e1-a363-f02cc86c3eec");
            var sut = new MessageJsonRepository(_file);
            var actual = sut.Get(id);
            Assert.Null(actual);

        }


    }
}
