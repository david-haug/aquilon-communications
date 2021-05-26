using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Application.Conversations.MarkMessagesAsRead;
using Aes.Communication.Application.Conversations.ToggleUserFlag;
using Aes.Communication.Application.Events;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Aes.Communication.Tests.Common.Fakes.Events;
using Aes.Communication.Tests.Common.Fakes.Messages;
using Xunit;

namespace Aes.Communication.Application.Tests.Conversations
{
    public class MarkMessagesAsReadHandlerShould
    {
        [Fact]
        public void AddUserMessageRead()
        {
            var factory = new ConversationFactory();
            var conversation = factory.Create();
            var repo = new MockConversationRepository();
            repo.Save(conversation);

            var msgToRead = conversation.Messages[0];
            var request = new MarkMessagesAsReadRequest
            {
                ConversationId = conversation.Id.ToString(),
                User = new User { UserId = 999 },
                OrganizationId = 2
            };

            var sut = new MarkMessagesAsReadHandler(new AppUser { UserId = 1 }, repo, new EventDispatcher(new StubEventRepository()));
            sut.Handle(request, new System.Threading.CancellationToken());

            var actual = repo.Get(conversation.Id).Messages.ToList().FirstOrDefault(m=>m.Id == msgToRead.Id);
            Assert.NotEmpty(actual.ReadByUsers);
        }

        //[Fact]
        //public void AddUserMessageReadForMultipleMessages()
        //{
        //    var factory = new ConversationFactory();
        //    var conversation = factory.Create();
        //    var repo = new MockConversationRepository();
        //    repo.Save(conversation);

        //    var msg1 = conversation.Messages[0];
        //    var msg2 = conversation.Messages[1];

        //    var request = new MarkMessagesAsReadRequest
        //    {
        //        ConversationId = conversation.Id.ToString(),
        //        User = new UserDto { UserId = 999 },
        //        OrganizationId = 2
        //    };

        //    var sut = new MarkMessagesAsReadHandler(new AppUser { UserId = 9999 }, repo, new EventDispatcher(new StubEventRepository()));
        //    sut.Handle(request, new System.Threading.CancellationToken());

        //    var savedConversation = repo.Get(conversation.Id);
        //    Assert.NotEmpty(savedConversation.Messages[0].ReadByUsers);
        //    Assert.NotEmpty(savedConversation.Messages[1].ReadByUsers);
        //}

        [Fact]
        public void MarkAllMessagesForAConversationAsRead()
        {
            //ARRANGE
            var conversationId = Guid.NewGuid();
            var user1 = new User { UserId = 1 };
            var user2 = new User { UserId = 2 };

            var org1 = new Organization { Id = 1, Name = "Org1" };
            var org2 = new Organization { Id = 2, Name = "Org2" };

            //conversation has 2 unread messages for cp (1 message is private and not included in count), 1 for org
            var conversation = Conversation.Load(conversationId,
                new EntityId("1", MessageEntityType.Invoice),
                DateTime.Now,
                1,
                new List<Message>
                {
                    Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                        "message 1", true, user1, DateTime.Now, null ,org1),
                    Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                        "message 2", false, user1, DateTime.Now, null ,org1),
                    Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                        "message 3", true, user1, DateTime.Now, null ,org1),
                    Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                        "message 4", true, user2, DateTime.Now, null ,org2)
                },
                new ConversationTopic("title", null),
                null,
                new EntityId("12345", MessageEntityType.TieOut),
                org1.Id,
                org2.Id);

            var repo = new MockConversationRepository();
            repo.Save(conversation);

            //user 2 will read messages
            var request = new MarkMessagesAsReadRequest
            {
                ConversationId = conversation.Id.ToString(),
                User = new User { UserId = user2.UserId },
                OrganizationId = 2
            };

            var sut = new MarkMessagesAsReadHandler(new AppUser { UserId = 9999 }, repo, new EventDispatcher(new StubEventRepository()));
            sut.Handle(request, new System.Threading.CancellationToken());
            var savedConversation = repo.Get(conversation.Id);

            var actual = savedConversation.GetUnreadMessageCountByOrganization(org2.Id);
            Assert.Equal(0, actual);
        }

        [Fact]
        public async void ThrowNotFoundExceptionGivenInvalidConversationId()
        {
            //ARRANGE
            var conversation = new ConversationFactory().Create();
            var repo = new MockConversationRepository();
            await repo.Save(conversation);


            var sut = new MarkMessagesAsReadHandler(new AppUser { UserId = 1 }, repo, new EventDispatcher(new StubEventRepository()));
            var request = new MarkMessagesAsReadRequest
            {
                ConversationId = Guid.NewGuid().ToString()
            };

            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        //[Fact]
        //public async void ThrowNotFoundExceptionGivenInvalidMessageId()
        //{
        //    //create a conversation
        //    var conversation = new ConversationFactory().Create();
        //    var repo = new MockConversationRepository();
        //    repo.Save(conversation);

        //    var sut = new MarkMessagesAsReadHandler(new AppUser { UserId = 1 }, repo, new EventDispatcher(new StubEventRepository()));
        //    var request = new MarkMessagesAsReadRequest
        //    {
        //        ConversationId = conversation.Id.ToString(),
        //        MessageIds = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }
        //    };

        //    await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        //}

        //[Fact]
        //public void DispatchMessageEvents()
        //{
        //    throw new NotImplementedException();
        //    ////add message to test repo
        //    //var repo = new MockMessageRepository();

        //    //var id = Guid.NewGuid();
        //    //var conversationGuid = Guid.NewGuid();
        //    //var body = "body";
        //    //var isPublic = false;
        //    //var userId = 2;
        //    //var date = DateTime.Now;
        //    //var entityId = new EntityId("1", MessageEntityType.Invoice);

        //    //var msg = Message.Load(id, conversationGuid, entityId, body, isPublic, userId, date, null);
        //    //repo.Add(msg);

        //    //var request = new MarkMessagesAsReadRequest
        //    //{
        //    //    MessageIds = new List<string> { id.ToString() }
        //    //};


        //    //var sut = new MarkMessagesAsReadHandler(new AppUser { UserId = 1 }, repo, new EventDispatcher(new StubEventRepository()));
        //    //sut.Handle(request, new System.Threading.CancellationToken());

        //    ////var actual = repo.Get(id);
        //    ////Assert.NotEmpty(actual.ReadByUsers);

        //    //Assert.Empty(Domain.DomainEvents.Events);
        //}
    }
}
