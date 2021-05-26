using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Conversations.UpdateTopic;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Xunit;

namespace Aes.Communication.Application.Tests.Conversations
{
    public class UpdateConversationTopicHandlerShould
    {
        [Fact]
        public void UpdateConversationTopic()
        {
            var sut = new UpdateConversationTopicHandler(new AppUser { UserId = 1 }, new StubConversationRepository());
            var request = new UpdateConversationTopicRequest
            {
                ConversationId = Guid.NewGuid().ToString(),
                Title = "New title",
                Attributes = new Dictionary<string, string> { {"Field1","Value1"} }
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.Equal(actual.Topic.Title,request.Title);
            Assert.Equal(actual.Topic.Attributes.Count, request.Attributes.Count);

        }

        [Fact]
        public async void ThrowNotFoundExceptionGivenInvalidId()
        {
            var sut = new UpdateConversationTopicHandler(new AppUser(), new MockConversationRepository());
            var request = new UpdateConversationTopicRequest { ConversationId = Guid.Empty.ToString() };
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));

        }
    }
}
