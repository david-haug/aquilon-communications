using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.GetConversation;
using Aes.Communication.Application.Conversations.GetConversations;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Xunit;

namespace Aes.Communication.Application.Tests.Conversations
{
    public class GetConversationsHandlerShould
    {
        [Fact]
        public void ReturnQueryResultWithConversationDtos()
        {
            var sut = new GetConversationsHandler(new FakeConversationReadOnlyRepository());
            var request = new GetConversationsRequest();
            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;

            Assert.NotEmpty(actual.Items);
        }

        [Fact]
        public void ReturnQueryResultWithCorrectTotalCount()
        {
            var repository = new FakeConversationReadOnlyRepository();
            var sut = new GetConversationsHandler(repository);
            var request = new GetConversationsRequest();

            var expected = repository.GetAllConversations().Count();
            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;

            Assert.Equal(expected, actual.TotalCount);
        }
    }
}
