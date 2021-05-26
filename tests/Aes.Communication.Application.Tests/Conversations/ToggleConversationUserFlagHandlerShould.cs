using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.ToggleUserFlag;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Xunit;

namespace Aes.Communication.Application.Tests.Conversations
{
    public class ToggleConversationUserFlagHandlerShould
    {
        [Fact]
        public void AddUserFlagGivenFlaggedByUserEqualsTrue()
        {
            var repo = new MockConversationRepository();
            var conversation = Conversation.Create(new Domain.Messages.EntityId("123", Domain.Messages.MessageEntityType.Invoice), 1,
                    new ConversationTopic("title"), new Domain.Messages.EntityId("123", Domain.Messages.MessageEntityType.TieOut),1,2);
            repo.Save(conversation);

            var sut = new ToggleConversationUserFlagHandler(new AppUser { UserId = 1 }, repo);
            var request = new ToggleConversationUserFlagRequest
            {
                FlaggedByUser = true,
                UserId = 1,
                ConversationId =conversation.Id.ToString()
            };

            sut.Handle(request, new System.Threading.CancellationToken());
            //get persisted conversation
            conversation = repo.Get(conversation.Id);

            Assert.NotEmpty(conversation.UserFlags);
        }

        [Fact]
        public void RemoveUserFlagGivenFlaggedByUserEqualsTrue()
        {
            var repo = new MockConversationRepository();
            var conversationId = Guid.NewGuid();
            //var conversation = Conversation.Load(conversationId,
            //    new EntityId("1", MessageEntityType.Invoice),
            //    DateTime.Now,
            //    1,
            //    new List<Message>
            //    {
            //        Message.Load(Guid.NewGuid(),conversationId,new EntityId("1", MessageEntityType.Invoice), "message 1",true,1,DateTime.Now,null),
            //        Message.Load(Guid.NewGuid(),conversationId,new EntityId("1", MessageEntityType.Invoice), "message 2",true,2,DateTime.Now,null)
            //    },
            //    new ConversationTopic("title",
            //        new Dictionary<string, string> { { "FieldA", "ValueA" }, { "FieldB", "ValueB" } }),
            //    new List<ConversationUserFlag>
            //    {
            //        new ConversationUserFlag(conversationId, 1, DateTime.Now),
            //        new ConversationUserFlag(conversationId, 2, DateTime.Now),
            //        new ConversationUserFlag(conversationId, 3, DateTime.Now),
            //    });
            var factory = new ConversationFactory();
            var conversation = factory.Create();

            repo.Save(conversation);
            var expected = conversation.UserFlags.Count - 1;

            var sut = new ToggleConversationUserFlagHandler(new AppUser { UserId = 1 }, repo);
            var request = new ToggleConversationUserFlagRequest
            {
                FlaggedByUser = false,
                UserId = 1,
                ConversationId = conversation.Id.ToString()
            };

            sut.Handle(request, new System.Threading.CancellationToken());
            //get persisted conversation
            conversation = repo.Get(conversation.Id);

            Assert.Equal(expected, conversation.UserFlags.Count);
        }

        [Fact]
        public async void ThrowNotFoundExceptionGivenInvalidId()
        {
            var sut = new ToggleConversationUserFlagHandler(new AppUser { UserId = 1 }, new StubConversationRepository());
            var request = new ToggleConversationUserFlagRequest
            {
                FlaggedByUser = true,
                UserId = 1,
                ConversationId = "123"
            };

            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
