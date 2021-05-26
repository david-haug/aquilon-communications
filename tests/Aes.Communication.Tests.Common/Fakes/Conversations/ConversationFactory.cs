using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;

namespace Aes.Communication.Tests.Common.Fakes.Conversations
{
    public class ConversationFactory
    {
        public Conversation Create()
        {
            return Create(Guid.NewGuid());
        }

        public Conversation Create(Guid uniqueId)
        {
            var conversationId = uniqueId;
            var user = new User
            {
                UserId = 1,
                FirstName = "Johnny",
                LastName = "Tester"
            };
            var user2 = new User
            {
                UserId = 2,
                FirstName = "Polly",
                LastName = "Person"
            };

            var org1 = new Organization
            {
                Id = 1,
                Name = "Manufactured Taste, Inc."
            };
            var org2 = new Organization
            {
                Id = 2,
                Name = "Industrial Carts Co."
            };

            return Conversation.Load(conversationId,
                new EntityId("1", MessageEntityType.Invoice),
                DateTime.Now,
                1,
                new List<Message>
                {
                    Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                        "message 1", true, user, DateTime.Now, null,org1),
                    Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                        "message 2", true, user2, DateTime.Now, null,org2)
                },
                new ConversationTopic("title",
                    new Dictionary<string, string> { { "FieldA", "ValueA" }, { "FieldB", "ValueB" } }),
                new List<ConversationUserFlag>
                {
                    new ConversationUserFlag(conversationId, 1, DateTime.Now),
                    new ConversationUserFlag(conversationId, 2, DateTime.Now),
                    new ConversationUserFlag(conversationId, 3, DateTime.Now)
                },
                new EntityId("12345", MessageEntityType.TieOut),
                org1.Id,
                org2.Id);


        }

        public Conversation Create(EntityId subject, EntityId parent)
        {
            var conversationId = Guid.NewGuid();
            var user = new User
            {
                UserId = 1,
                FirstName = "Johnny",
                LastName = "Tester"
            };
            var user2 = new User
            {
                UserId = 2,
                FirstName = "Polly",
                LastName = "Person"
            };

            var org1 = new Organization
            {
                Id = 1,
                Name = "Manufactured Taste, Inc."
            };
            var org2 = new Organization
            {
                Id = 2,
                Name = "Industrial Carts Co."
            };

            return Conversation.Load(conversationId,
                subject,
                DateTime.Now,
                1,
                new List<Message>
                {
                    Message.Load(Guid.NewGuid(), conversationId, subject,
                        "message 1", true, user, DateTime.Now, null,org1),
                    Message.Load(Guid.NewGuid(), conversationId, subject,
                        "message 2", true, user2, DateTime.Now, null,org2)
                },
                new ConversationTopic("title",
                    new Dictionary<string, string> { { "FieldA", "ValueA" }, { "FieldB", "ValueB" } }),
                new List<ConversationUserFlag>
                {
                    new ConversationUserFlag(conversationId, 1, DateTime.Now),
                    new ConversationUserFlag(conversationId, 2, DateTime.Now),
                    new ConversationUserFlag(conversationId, 3, DateTime.Now)
                },
                parent,
                org1.Id,
                org2.Id);


        }
    }
}
