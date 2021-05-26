using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.GetConversation;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Xunit;

namespace Aes.Communication.Application.Tests.Conversations
{
    public class GetConversationHandlerShould
    {
        [Fact]
        public async Task ThrowNotFoundExceptionGivenInvalidId()
        {
            var sut = new GetConversationHandler(new MockConversationReadOnlyRepository());
            var request = new GetConversationRequest {ConversationId = "123"};
            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public void ReturnConversationDtoGivenValidId()
        {
            var sut = new GetConversationHandler(new FakeConversationReadOnlyRepository());
            var request = new GetConversationRequest { ConversationId = "cf7278d6-9e75-44d8-afc6-b150c7faabdc" };
            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.Equal(request.ConversationId, actual.Id.ToString());
        }

    }
}
