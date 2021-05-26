using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Xunit;

namespace Aes.Communication.Application.Tests.Conversations
{
    public class AddConversationMessageHandlerShould
    {
        private User _user = new User
        {
            UserId = 1,
            FirstName = "Johnny",
            LastName = "Tester",
            Email = "jtester@aquiloninc.com"
        };

        private Organization _organization = new Organization
        {
            Id = 1,
            Name = "Manufactured Taste, Inc."
        };

        [Fact]
        public void ReturnConversationMessageDtoGivenValidRequest()
        {
            var conversation = new ConversationFactory().Create();
            var repo = new MockConversationRepository();
            repo.Save(conversation);

            var sut = new AddConversationMessageHandler(new AppUser { UserId = 1 }, repo);
            var request = new AddConversationMessageRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationId = conversation.Id.ToString(),
                Subject = new EntityId("1",MessageEntityType.TieOut),
                User = _user,
                Organization = _organization
            };
            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.IsType<ConversationMessageDto>(actual);
        }

        [Fact]
        public async void ThrowNotFoundExceptionGivenInvalidId()
        {
            var sut = new AddConversationMessageHandler(new AppUser { UserId = 1 }, CreateMockRepository());
            var request = new AddConversationMessageRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationId = "123",
                Subject = new EntityId("1", MessageEntityType.TieOut),
                User = _user,
                Organization = _organization
            };

            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public void ReturnConversationMessageDtoWithSubject()
        {
            var conversation = new ConversationFactory().Create();
            var repo = new MockConversationRepository();
            repo.Save(conversation);

            var sut = new AddConversationMessageHandler(new AppUser { UserId = 1 }, repo);
            var entityId = new EntityId("abc123",MessageEntityType.Deal);
            var request = new AddConversationMessageRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationId = conversation.Id.ToString(),
                Subject = entityId,
                User = _user,
                Organization = _organization
            };
            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;

            Assert.Equal(entityId.Id, actual.Subject.Id);
            Assert.Equal(entityId.EntityType, actual.Subject.EntityType);
        }

        [Fact]
        public void UseConversationSubjectGivenNullRequestSubject()
        {
            var conversation = new ConversationFactory().Create();
            var repo = new MockConversationRepository();
            repo.Save(conversation);

            var sut = new AddConversationMessageHandler(new AppUser { UserId = 1 }, repo);
            var request = new AddConversationMessageRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationId = conversation.Id.ToString(),
                User = _user,
                Organization = _organization
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.Equal(conversation.Subject.Id, actual.Subject.Id);
            Assert.Equal(conversation.Subject.EntityType, actual.Subject.EntityType);
        }

        [Fact]
        public void UseRequestSubjectWhenGiven()
        {
            var conversation = new ConversationFactory().Create();
            var repo = new MockConversationRepository();
            repo.Save(conversation);

            var sut = new AddConversationMessageHandler(new AppUser { UserId = 1 }, repo);
            var request = new AddConversationMessageRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationId = conversation.Id.ToString(),
                User = _user,
                Organization = _organization,
                Subject = new EntityId(Guid.NewGuid().ToString(),MessageEntityType.Deal)
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.NotEqual(conversation.Subject.Id, actual.Subject.Id);
            Assert.NotEqual(conversation.Subject.EntityType, actual.Subject.EntityType);
        }

        [Fact]
        public void ReturnAttachmentsGivenValidRequest()
        {
            var conversation = new ConversationFactory().Create();
            var repo = new MockConversationRepository();
            repo.Save(conversation);

            var sut = new AddConversationMessageHandler(new AppUser { UserId = 1 }, repo);
            var request = new AddConversationMessageRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationId = conversation.Id.ToString(),
                Subject = new EntityId("1", MessageEntityType.TieOut),
                User = _user,
                Organization = _organization,
                Attachments = new List<Attachment>
                {
                    new Attachment { FileId = "1",FileName="file1.pdf",FilePath="http://file1",FileSize = "100MB"},
                    new Attachment { FileId = "2",FileName="file2.pdf",FilePath="http://file2",FileSize = "200MB"},
                }
            };
            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.Equal(request.Attachments.Count(), actual.Attachments.Count());
        }

        [Fact]
        public void SaveUserEmailWhenGiven()
        {
            var conversation = new ConversationFactory().Create();
            var repo = new MockConversationRepository();
            repo.Save(conversation);

            var sut = new AddConversationMessageHandler(new AppUser { UserId = 1 }, repo);
            var request = new AddConversationMessageRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationId = conversation.Id.ToString(),
                Subject = new EntityId("1", MessageEntityType.TieOut),
                User = _user,
                Organization = _organization,
                Attachments = new List<Attachment>
                {
                    new Attachment { FileId = "1",FileName="file1.pdf",FilePath="http://file1",FileSize = "100MB"},
                    new Attachment { FileId = "2",FileName="file2.pdf",FilePath="http://file2",FileSize = "200MB"},
                }
            };
            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.Equal(request.User.Email, actual.User.Email);
        }

        private MockConversationRepository CreateMockRepository()
        {
            var conversation = new ConversationFactory().Create();
            var repository = new MockConversationRepository();
            repository.Save(conversation);

            return repository;
        }

    }
}
