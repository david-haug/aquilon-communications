using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using Aes.Communication.Infrastructure.CosmosDataAccess.Repositories;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Aes.Communication.Tests.Common.Fakes.Users;
using Xunit;

namespace Aes.Communication.Infrastructure.Tests.CosmosDataAccess
{
    public class ConversationRepositoryShould
    {
        private ConversationRepository _repository = new ConversationRepository(Settings.Cosmos.ConnectionString,
            Settings.Cosmos.CommunicationsDatabase, Settings.Cosmos.ConversationsCollection);
        [Fact]
        public void ReturnConversationGivenValidId()
        {
            var sut = _repository;
            var actual = sut.Get(Guid.Parse(Settings.Cosmos.ValidConversationId));
            Assert.NotNull(actual);
        }

        [Fact]
        public async void SaveNewConversation()
        {
            var conversation = new ConversationFactory().Create();
            var sut = _repository;
            await sut.Save(conversation);

            var actual = new Guid();
            var saved = sut.Get(conversation.Id);
            if (saved != null)
                actual = saved.Id;

            Assert.Equal(conversation.Id, actual);
        }

        [Fact]
        public async void SaveExistingConversation()
        {
            var id = Guid.Parse(Settings.Cosmos.ValidConversationId);
            var sut = _repository;
            var conversation = sut.Get(id);

            var expected = conversation.Messages.Count;
            var user = new UserFactory().Create();
            var org = new Organization
            {
                Id = 1,
                Name = "Manufactured Taste, Inc."
            };

            //add message
            conversation.AddMessage(Message.Create(conversation, new EntityId("1", MessageEntityType.Invoice), "body", true, user, org));

            await sut.Save(conversation);
            var actual = sut.Get(id).Messages.Count;
            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public async void SaveAttachmentsWhenUpdatingConversation()
        {
            var id = Guid.Parse(Settings.Cosmos.ValidConversationId);
            var sut = _repository;
            var conversation = sut.Get(id);

            //add message
            var user = new UserFactory().Create();
            var org = new Organization
            {
                Id = 1,
                Name = "Manufactured Taste, Inc."
            };

            var msg = Message.Create(conversation, new EntityId("1", MessageEntityType.Invoice), "body", true, user,
                org);
            msg.Attachments = new List<Attachment>
            {
                new Attachment {FileId = "1", FileName = "file1.pdf", FilePath = "http://file1", FileSize = "100MB"},
                new Attachment {FileId = "2", FileName = "file2.pdf", FilePath = "http://file2", FileSize = "200MB"}
            };
            conversation.AddMessage(msg);

            var expected = msg.Attachments.Count();
            await sut.Save(conversation);


            var saved = sut.Get(conversation.Id);
            var savedMsg = saved.Messages.FirstOrDefault(m => m.Id == msg.Id);

            Assert.Equal(expected, savedMsg.Attachments.Count());
        }

        [Fact]
        public void ReturnConversationGivenValidSubject()
        {
            var sut = _repository;
            var actual = sut.GetBySubject(new EntityId("1", MessageEntityType.Invoice));
            Assert.NotNull(actual);
        }
        [Fact]
        public void ReturnNullGivenInvalidSubject()
        {
            var sut = _repository;
            var actual = sut.GetBySubject(new EntityId("", MessageEntityType.Dispute));
            Assert.Null(actual);
        }
    }
}
