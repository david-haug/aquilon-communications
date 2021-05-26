using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Infrastructure.Conversations;
using Xunit;

namespace Aes.Communication.Infrastructure.Tests.Conversations
{
    public class ConversationReadOnlyJsonRepositoryShould
    {
        private string _file = Settings.Json.ConversationsFilePath;
        private string _validId = Settings.Json.ValidConversationId;

        [Fact]
        public void ReturnConversationDtoGivenValidId()
        {
            var sut = new ConversationReadOnlyJsonRepository(_file, new AppUser { UserId = 1 });
            var id = _validId;
            var actual = sut.GetConversation(id);
            Assert.Equal(id, actual.Id.ToString());
            Assert.IsType<ConversationDto>(actual);
        }

        [Fact]
        public void ReturnNullGivenInvalidId()
        {
            var sut = new ConversationReadOnlyJsonRepository(_file, new AppUser { UserId = 1 });
            var id = "123";
            Assert.Null(sut.GetConversation(id));
        }

        [Fact]
        public void ReturnAttributesDtoGivenValidId()
        {
            var sut = new ConversationReadOnlyJsonRepository(_file, new AppUser { UserId = 1 });
            var id = _validId;
            var actual = sut.GetConversation(id);
            Assert.NotEmpty(actual.Topic.Attributes);
        }

        [Fact]
        public void ReturnAllConversations()
        {
            var sut = new ConversationReadOnlyJsonRepository(_file, new AppUser { UserId = 1 });
            var actual = sut.GetAllConversations();
            Assert.NotEmpty(actual);

        }
    }
}
