using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Conversations.ChangeParent;
using Aes.Communication.Application.Conversations.MarkMessagesAsRead;
using Aes.Communication.Application.Events;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Aes.Communication.Tests.Common.Fakes.Events;
using Xunit;

namespace Aes.Communication.Application.Tests.Conversations
{
    public class ChangeConversationParentHandlerShould
    {
        [Fact]
        public void ChangeParent()
        {
            var conversation = new ConversationFactory().Create();
            var repo = new MockConversationRepository();
            repo.Save(conversation);

            var request = new ChangeConversationParentRequest
            {
                ConversationId = conversation.Id.ToString(),
                Parent = new EntityId("newid",MessageEntityType.TieOut)
            };

            var sut = new ChangeConversationParentHandler(new AppUser { UserId = 1 }, repo);
            sut.Handle(request, new System.Threading.CancellationToken());

            //refetch conversation
            var actual = repo.Get(conversation.Id);

            Assert.Equal(request.Parent.Id,actual.Parent.Id);
        }

        [Fact]
        public async void ThrowNotFoundExceptionGivenInvalidConversationId()
        {
            var sut = new ChangeConversationParentHandler(new AppUser { UserId = 1 }, new MockConversationRepository());
            var request = new ChangeConversationParentRequest
            {
                ConversationId = "000000000",
                Parent = new EntityId("newid", MessageEntityType.TieOut)
            };

            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

    }
}
