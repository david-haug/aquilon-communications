using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Xunit;

namespace Aes.Communication.Application.Tests.Conversations
{
    public class CreateConversationHandlerShould
    {
        [Fact]
        public void ReturnConversationDtoGivenValidRequest()
        {
            var sut = new CreateConversationHandler(new AppUser { UserId = 1 }, new StubConversationRepository());
            var request = new CreateConversationRequest
            {
                Subject = new EntityId("1",MessageEntityType.Invoice),
                Parent = new EntityId("12345",MessageEntityType.TieOut),
                Title = "Testing Conversation",
                HeaderAttributes = new Dictionary<string,string> { {"FieldA","ValueA"} },
                CreatedByUserId = 1,
                OrganizationId = 1,
                CounterpartyId = 2
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.IsType<ConversationDto>(actual);
        }

        [Fact]
        public void ReturnConversationDtoWithTwoAttributesGivenValidRequest()
        {
            var sut = new CreateConversationHandler(new AppUser { UserId = 1 }, new StubConversationRepository());
            var request = new CreateConversationRequest
            {
                Subject = new EntityId("1", MessageEntityType.Invoice),
                Parent = new EntityId("12345", MessageEntityType.TieOut),
                Title = "Title",
                HeaderAttributes = new Dictionary<string, string> { { "FieldA", "ValueA" }, { "FieldB", "ValueB" } },
                CreatedByUserId = 1,
                OrganizationId = 1,
                CounterpartyId = 2
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.Equal(actual.Topic.Attributes.Count, request.HeaderAttributes.Count);
        }

        [Fact]
        public void CreateConversationWithMessages()
        {
            var sut = new CreateConversationHandler(new AppUser { UserId = 1 }, new StubConversationRepository());

            var messages = new List<AddConversationMessageRequest>
            {
                new AddConversationMessageRequest
                {
                    Body = "body",
                    IsPublic = true,
                    Subject = new EntityId("1",MessageEntityType.TieOut),
                    User = new User { UserId = 1 },
                    Organization = new Organization { Id = 1 }
                },
                new AddConversationMessageRequest
                {
                    Body = "body2",
                    IsPublic = true,
                    Subject = new EntityId("1",MessageEntityType.TieOut),
                    User = new User { UserId = 2 },
                    Organization = new Organization { Id = 2 }
                },
            };

            var request = new CreateConversationRequest
            {
                Subject = new EntityId("1", MessageEntityType.Invoice),
                Parent = new EntityId("12345", MessageEntityType.TieOut),
                Title = "Title",
                HeaderAttributes = new Dictionary<string, string> { { "FieldA", "ValueA" }, { "FieldB", "ValueB" } },
                CreatedByUserId = 1,
                OrganizationId = 1,
                CounterpartyId = 2,
                Messages = messages
            };

            var actual = sut.Handle(request, new System.Threading.CancellationToken()).Result;
            Assert.Equal(actual.Messages.Count(), messages.Count);
        }

        [Fact]
        public async void ThrowsExceptionWhenMessagesContainInvalidMessageRequest()
        {
            var sut = new CreateConversationHandler(new AppUser { UserId = 1 }, new StubConversationRepository());

            var request = new CreateConversationRequest
            {
                Subject = new EntityId("1", MessageEntityType.Invoice),
                Parent = new EntityId("12345", MessageEntityType.TieOut),
                Title = "Title",
                HeaderAttributes = new Dictionary<string, string> { { "FieldA", "ValueA" }, { "FieldB", "ValueB" } },
                CreatedByUserId = 1,
                OrganizationId = 1,
                CounterpartyId = 2
            };

            request.Messages = new List<AddConversationMessageRequest>
            {
                new AddConversationMessageRequest
                {
                    Body = "body",
                    IsPublic = true,
                    Subject = new EntityId("1",MessageEntityType.TieOut),
                    User = new Domain.ValueObjects.User { UserId = 1 },
                    Organization = new Domain.ValueObjects.Organization { Id = 3 } //org not in conversation
                }
            };

            await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, new System.Threading.CancellationToken()));
        }
    }
}
