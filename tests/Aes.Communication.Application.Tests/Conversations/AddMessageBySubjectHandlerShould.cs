using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.AddMessageByConversationSubject;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Xunit;

namespace Aes.Communication.Application.Tests.Conversations
{
    public class AddMessageBySubjectHandlerShould
    {
        [Fact]
        public void ReturnConversationMessageDtoGivenValidRequest()
        {
            var sut = new AddMessageByConversationSubjectHandler(new AppUser { UserId = 1 }, CreateMockRepository());
            var request = new AddMessageByConversationSubjectRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationSubject = new EntityId("123",MessageEntityType.Dispute),
                User = _user,
                Organization = _organization
            };
            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.IsType<ConversationMessageDto>(actual);
        }

        [Fact]
        public async void ThrowNotFoundExceptionGivenInvalidConversationSubjectId()
        {
            var sut = new AddMessageByConversationSubjectHandler(new AppUser { UserId = 1 }, CreateMockRepository());
            var request = new AddMessageByConversationSubjectRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationSubject = new EntityId("999", MessageEntityType.Dispute),
                User = _user,
                Organization = _organization
            };

            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public async void ThrowBadRequestExceptionGivenNoConversationSubject()
        {
            var sut = new AddMessageByConversationSubjectHandler(new AppUser { UserId = 1 }, CreateMockRepository());
            var request = new AddMessageByConversationSubjectRequest
            {
                Body = "body",
                IsPublic = true,
                User = _user,
                Organization = _organization
            };

            await Assert.ThrowsAsync<BadRequestException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }

        [Fact]
        public void UseConversationSubjectGivenNullRequestSubject()
        {
            var sut = new AddMessageByConversationSubjectHandler(new AppUser { UserId = 1 }, CreateMockRepository());
            var request = new AddMessageByConversationSubjectRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationSubject = new EntityId("123", MessageEntityType.Dispute),
                User = _user,
                Organization = _organization
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.Equal(request.ConversationSubject.Id, actual.Subject.Id);
            Assert.Equal(request.ConversationSubject.EntityType, actual.Subject.EntityType);
        }
        [Fact]
        public void UseMessageSubjectWhenGiven()
        {
            var sut = new AddMessageByConversationSubjectHandler(new AppUser { UserId = 1 }, CreateMockRepository());
            var request = new AddMessageByConversationSubjectRequest
            {
                Body = "body",
                IsPublic = true,
                ConversationSubject = new EntityId("123", MessageEntityType.Dispute),
                MessageSubject = new EntityId("456", MessageEntityType.Deal),
                User = _user,
                Organization = _organization
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.Equal(request.MessageSubject.Id, actual.Subject.Id);
            Assert.Equal(request.MessageSubject.EntityType, actual.Subject.EntityType);
        }

        private MockConversationRepository CreateMockRepository()
        {
            var conversation = new ConversationFactory().Create(new EntityId("123", MessageEntityType.Dispute), new EntityId("12345", MessageEntityType.TieOut));
            var repository = new MockConversationRepository();
            repository.Save(conversation);

            return repository;
        }
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
    }
}
