using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using Aes.Communication.Tests.Common.Fakes.Conversations;
using Xunit;

namespace Aes.Communication.Domain.Tests.Conversations
{
    public class ConversationShould
    {
        [Fact]
        public void LoadGivenValidArguments()
        {
            //arrange
            var id = Guid.NewGuid();
            var subject = new EntityId("1", MessageEntityType.Invoice);
            var dateCreated = DateTime.Now;
            var createdByUserId =1;
            var messages = new List<Message>();
            var topic = new ConversationTopic("Hello");
            var parent = new EntityId("12345", MessageEntityType.TieOut);
            var orgId = 1;
            var counterpartyId = 2;
            //act
            var sut = Conversation.Load(id, subject, dateCreated, createdByUserId, messages, topic, null, parent, orgId, counterpartyId);

            //assert
            Assert.Equal(id, sut.Id);
            Assert.Equal(subject, sut.Subject);
            Assert.Equal(dateCreated, sut.DateCreated);
            Assert.Equal(1, sut.CreatedByUserId);
            Assert.Equal(messages, sut.Messages);
            Assert.Equal(parent, sut.Parent);
            Assert.Equal(orgId, sut.OrganizationId);
            Assert.Equal(counterpartyId, sut.CounterpartyId);
        }

        [Fact]
        public void CreateGivenValidArguments()
        {
            //arrange
            var subject = new EntityId("1", MessageEntityType.Invoice);
            var createdByUserId = 1;
            var topic = new ConversationTopic("Hello");
            var parent = new EntityId("12345", MessageEntityType.TieOut);
            var orgId = 1;
            var counterpartyId = 2;
            //act
            var sut = Conversation.Create(subject, createdByUserId, topic,parent, orgId, counterpartyId);

            //assert
            Assert.Equal(subject, sut.Subject);
            Assert.Equal(1, sut.CreatedByUserId);
        }

        [Fact]
        public void ThrowArgumentExceptionGivenInvalidSubject()
        {
            Assert.Throws<ArgumentException>(() => Conversation.Create(new EntityId("", MessageEntityType.Invoice), 1, new ConversationTopic("Title"), new EntityId("12345", MessageEntityType.TieOut), 1, 1));
            Assert.Throws<ArgumentException>(() => Conversation.Create(new EntityId("1", MessageEntityType.Unknown), 1, new ConversationTopic("Title"), new EntityId("12345", MessageEntityType.TieOut),1,1));
        }
        [Fact]
        public void ThrowArgumentExceptionGivenInvalidUserId()
        {
            Assert.Throws<ArgumentException>(() => Conversation.Create(new EntityId("1", MessageEntityType.Invoice), 0, new ConversationTopic("Title"), new EntityId("12345", MessageEntityType.TieOut),1,1));
        }

        [Fact]
        public void AddMessage()
        {
            var entityId = new EntityId("1", MessageEntityType.Invoice);
            var sut = Conversation.Load(Guid.NewGuid(),entityId,DateTime.Now,1,null, new ConversationTopic("Title"),null, new EntityId("12345", MessageEntityType.TieOut),1,1);
            sut.AddMessage(Message.Create(sut, entityId,"body", true, _user, _organization));

            Assert.NotEmpty(sut.Messages);
        }

        [Fact]
        public void AddMessageWithAttachments()
        {
            var entityId = new EntityId("1", MessageEntityType.Invoice);
            var sut = Conversation.Load(Guid.NewGuid(), entityId, DateTime.Now, 1, null, new ConversationTopic("Title"), null, new EntityId("12345", MessageEntityType.TieOut), 1, 1);

            var msg = Message.Create(sut, entityId, "body", true, _user, _organization);
            msg.Attachments = new List<Attachment>
            {
                new Attachment {FileId = "1", FileName = "fileName", FilePath = "filePath", FileSize = "256 kb"}
            };
            sut.AddMessage(msg);

            Assert.NotEmpty(sut.Messages[0].Attachments);
        }

        [Fact]
        public void HaveMoreThanOneMessageWhenAddingToExisting()
        {
            var entityId = new EntityId("1", MessageEntityType.Invoice);
            var messages = new List<Message>();
            messages.Add(Message.Load(Guid.NewGuid(), Guid.NewGuid(), entityId, "msg1", true, _user, DateTime.Now,null, _organization));
            
            var sut = Conversation.Load(Guid.NewGuid(), entityId, DateTime.Now, 1, messages, new ConversationTopic("Title"),null, new EntityId("12345", MessageEntityType.TieOut),1,1);
            sut.AddMessage(Message.Create(sut, entityId, "msg2", true, _user, _organization));

            Assert.Equal(2, sut.Messages.Count);
        }

        [Fact]
        public void ThrowApplicationExceptionGivenOrganizationIdNotInConversationWhenAddingMessage()
        {
            var entityId = new EntityId("1", MessageEntityType.Invoice);
            var sut = Conversation.Load(Guid.NewGuid(), entityId, DateTime.Now, 1, null, new ConversationTopic("Title"), null, new EntityId("12345", MessageEntityType.TieOut), 1, 2);

            Assert.Throws<ApplicationException>(() => sut.AddMessage(Message.Create(sut, entityId, "body", true, _user, new Organization { Id = 3 })));
        }

        [Fact]
        public void ThrowArgumentExceptionGivenNoTitle()
        {
            Assert.Throws<ArgumentException>(() => Conversation.Create(new EntityId("1", MessageEntityType.Invoice), 1, new ConversationTopic(""), new EntityId("12345", MessageEntityType.TieOut),1,1));
        }

        [Fact]
        public void CreateTopicWithAttributesEqualToGivenAttributes()
        {
            var entityId = new EntityId("1", MessageEntityType.Invoice);
            var topic = new ConversationTopic("Title", new Dictionary<string,string> { {"FieldA","ValueA"}, { "FieldB", "ValueB"} });
            var sut = Conversation.Load(Guid.NewGuid(), entityId, DateTime.Now, 1, null, topic,null, new EntityId("12345", MessageEntityType.TieOut),1,1);
            sut.AddMessage(Message.Create(sut, entityId, "msg2", true, _user, _organization));

            Assert.Equal(2, sut.Topic.Attributes.Count);
        }

        [Fact]
        public void UpdateTopicGivenValidParameters()
        {
            var topic = new ConversationTopic("Title", new Dictionary<string, string> { { "FieldA", "ValueA" }, { "FieldB", "ValueB" } });
            var sut = Conversation.Load(Guid.NewGuid(), new EntityId("1", MessageEntityType.Invoice), DateTime.Now, 1, null, topic,null, new EntityId("12345", MessageEntityType.TieOut),1,1);

            var newTopic = new ConversationTopic("Title1", new Dictionary<string, string> { { "FieldC", "ValueC" } });
            sut.UpdateTopic(newTopic);

            Assert.Equal(newTopic.Title, sut.Topic.Title);
            Assert.Equal(newTopic.Attributes.Count, sut.Topic.Attributes.Count);

        }

        [Fact]
        public void ThrowArgumentExceptionWhenUpdatingTopicWithNoTitle()
        {
            var sut = Conversation.Load(Guid.NewGuid(), new EntityId("1", MessageEntityType.Invoice), DateTime.Now, 1, null, new ConversationTopic("title"),null, new EntityId("12345", MessageEntityType.TieOut),1,1);
            Assert.Throws<ArgumentException>(() => sut.UpdateTopic(new ConversationTopic(null)));
        }

        [Fact]
        public void ReplaceExistingAttributesWhenUpdatingTopic()
        {
            var topic = new ConversationTopic("Title", new Dictionary<string, string> { { "FieldA", "ValueA" }, { "FieldB", "ValueB" } });
            var sut = Conversation.Load(Guid.NewGuid(), new EntityId("1", MessageEntityType.Invoice), DateTime.Now, 1, null, topic,null, new EntityId("12345", MessageEntityType.TieOut),1,1);

            var newTopic = new ConversationTopic("Title", new Dictionary<string, string> { { "FieldC", "ValueC" } });
            Assert.NotEqual(newTopic.Attributes.Count, sut.Topic.Attributes.Count);
        }

        [Fact]
        public void LoadUserFlagsWhenGivenUserFlags()
        {
            var flags = new List<ConversationUserFlag> {new ConversationUserFlag(Guid.NewGuid(), 1, DateTime.Now)};
            var sut = Conversation.Load(Guid.NewGuid(), new EntityId("1", MessageEntityType.Invoice), DateTime.Now, 1, null, new ConversationTopic("title"), flags, new EntityId("12345", MessageEntityType.TieOut),1,1);

            Assert.NotEmpty(sut.UserFlags);
        }

        [Fact]
        public void AddUserFlagGivenValidUserId()
        {
            var sut = new ConversationFactory().Create();
            var expected = sut.UserFlags.Count + 1;
            sut.AddUserFlag(99999);

            Assert.Equal(expected, sut.UserFlags.Count);
        }

        [Fact]
        public void NotAddDuplicateUserFlag()
        {
            var sut = new ConversationFactory().Create();
            var expected = sut.UserFlags.Count;
            sut.AddUserFlag(2);

            Assert.Equal(expected, sut.UserFlags.Count);
        }

        [Fact]
        public void ThrowArgumentExceptionWhenAddingUserFlagGivenInvalidUserId()
        {
            var sut = new ConversationFactory().Create();
            Assert.Throws<ArgumentException>(() => sut.AddUserFlag(0));
        }
        
        [Fact]
        public void ThrowArgumentExceptionWhenRemovingUserFlagGivenInvalidUserId()
        {
            var sut = new ConversationFactory().Create();
            Assert.Throws<ArgumentException>(() => sut.RemoveUserFlag(0));
        }

        [Fact]
        public void RemoveUserFlagGivenValidUserId()
        {
            var sut = new ConversationFactory().Create();
            var expected = sut.UserFlags.Count -1;
            sut.RemoveUserFlag(2);

            Assert.Equal(expected, sut.UserFlags.Count);
        }

        [Fact]
        public void ChangeParentGivenValidParameters()
        {
            var factory = new ConversationFactory();
            var sut = factory.Create();

            var originalParent = sut.Parent;
            var parent = new EntityId("UNIQUEID_1", MessageEntityType.TieOut);
            sut.ChangeParent(parent);

            Assert.NotEqual(originalParent.Id, sut.Parent.Id);
        }

        [Fact]
        public void ThrowArgumentExceptionGivenInvalidOrganizationId()
        {
            Assert.Throws<ArgumentException>(() => 
                Conversation.Create(new EntityId("1", MessageEntityType.Invoice), 1, new ConversationTopic("Title"), new EntityId("12345", MessageEntityType.TieOut), 0, 1));
        }

        [Fact]
        public void ThrowArgumentExceptionGivenInvalidCounterpartyId()
        {
            Assert.Throws<ArgumentException>(() => Conversation.Create(new EntityId("1", MessageEntityType.Invoice), 1, new ConversationTopic("Title"), new EntityId("12345", MessageEntityType.TieOut), 1, 0));
        }

        [Fact]
        public void ReturnUnreadMessageCountForOrganization()
        {
            var conversationId = Guid.NewGuid();
            var user1 = new User { UserId = 1 };
            var user2 = new User { UserId = 2 };

            var org1 = new Organization { Id = 1, Name = "Org1" };
            var org2 = new Organization { Id = 2, Name = "Org2" };

            
            var expected = 1; //message 1 is read, message 2 isn't
            var sut = Conversation.Load(conversationId,
                 new EntityId("1", MessageEntityType.Invoice),
                 DateTime.Now,
                 1,
                 new List<Message>
                 {
                    Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                        "message 1", true, user1, DateTime.Now, new List<UserMessageRead>
                        {
                            new UserMessageRead(user2,org2.Id,DateTime.Now)
                        },org1),
                     Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                         "message 2", true, user1, DateTime.Now, null,org1)
                 },
                 new ConversationTopic("title",null),
                 null,
                 new EntityId("12345", MessageEntityType.TieOut),
                 org1.Id,
                 org2.Id);


            Assert.Equal(expected, sut.GetUnreadMessageCountByOrganization(org2.Id));
        }

        [Fact]
        public void NotIncludePrivateMessagesInUnreadMessageCount()
        {
            var conversationId = Guid.NewGuid();
            var user1 = new User { UserId = 1 };
            var user2 = new User { UserId = 2 };

            var org1 = new Organization { Id = 1, Name = "Org1" };
            var org2 = new Organization { Id = 2, Name = "Org2" };

            var sut = Conversation.Load(conversationId,
                new EntityId("1", MessageEntityType.Invoice),
                DateTime.Now,
                1,
                new List<Message>
                {
                    Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                        "message 1", false, user1, DateTime.Now, null ,org1)
                },
                new ConversationTopic("title", null),
                null,
                new EntityId("12345", MessageEntityType.TieOut),
                org1.Id,
                org2.Id);


            Assert.Equal(0, sut.GetUnreadMessageCountByOrganization(org2.Id));
        }

        [Fact]
        public void NotMarkPrivateMessageAsRead()
        {
            var conversationId = Guid.NewGuid();
            var user1 = new User { UserId = 1 };
            var user2 = new User { UserId = 2 };

            var org1 = new Organization { Id = 1, Name = "Org1" };
            var org2 = new Organization { Id = 2, Name = "Org2" };

            var message = Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                "message 1", false, user1, DateTime.Now, null, org1);

            var sut = Conversation.Load(conversationId,
                new EntityId("1", MessageEntityType.Invoice),
                DateTime.Now,
                1,
                new List<Message> { message },
                new ConversationTopic("title", null),
                null,
                new EntityId("12345", MessageEntityType.TieOut),
                org1.Id,
                org2.Id);

            sut.MarkMessageAsRead(message, user2, org2.Id);
            Assert.Empty(message.ReadByUsers);
        }


        #region Messages
        [Fact]
        public void AddToReadByUsersIfOrganizationAndUserIdAreUnique()
        {
            var sut = new ConversationFactory().Create();
            var msgToRead = sut.Messages[0];
            sut.MarkMessageAsRead(msgToRead, new User { UserId = 999 }, 2);
            sut.MarkMessageAsRead(msgToRead, new User { UserId = 123 }, 2);
            Assert.Equal(2,msgToRead.ReadByUsers.Count);
        }

        [Fact]
        public void AddToReadByUsersIfOrganizationAndUserIdAreUniqueAndConversationOrgIdSameAsUser()
        {

            var conversationId = Guid.NewGuid();
            var user1 = new User { UserId = 1 };
            var user2 = new User { UserId = 2 };

            var org1 = new Organization { Id = 1, Name = "Org1" };
            var org2 = new Organization { Id = 2, Name = "Org2" };

            //message from org2
            var message = Message.Load(Guid.NewGuid(), conversationId, new EntityId("1", MessageEntityType.Invoice),
                "message 1", true, user2, DateTime.Now, null, org2);

            //conversation created by org1
            var sut = Conversation.Load(conversationId,
                new EntityId("1", MessageEntityType.Invoice),
                DateTime.Now,
                1,
                new List<Message> { message },
                new ConversationTopic("title", null),
                null,
                new EntityId("12345", MessageEntityType.TieOut),
                org1.Id,
                org2.Id);

            sut.MarkMessageAsRead(message, user1, org1.Id);
            Assert.Single(message.ReadByUsers);
        }

        [Fact]
        public void NotAddDuplicateUserAndOrganizationToReadByUsers()
        {
            var sut = new ConversationFactory().Create();
            var msgToRead = sut.Messages[0];
            sut.MarkMessageAsRead(msgToRead, new User { UserId = 999 }, 2);
            sut.MarkMessageAsRead(msgToRead, new User { UserId = 999 }, 2);
            Assert.Single(msgToRead.ReadByUsers);
        }

        [Fact]
        public void NotAddToReadByUsersIfSameOrganizationAsMessage()
        {
            var sut = new ConversationFactory().Create();
            var msgToRead = sut.Messages[0];
            sut.MarkMessageAsRead(msgToRead, new User { UserId = 999 }, msgToRead.Organization.Id);
            Assert.Empty(msgToRead.ReadByUsers);
        }


        [Fact]
        public void ThrowApplicationExceptionGivenOrganizationIdNotInConversationWhenMarkingAsRead()
        {
            var sut = new ConversationFactory().Create();
            var msgToRead = sut.Messages[0];
            Assert.Throws<ApplicationException>(() => sut.MarkMessageAsRead(msgToRead, new User { UserId = 999 }, 9999999));
        }

        #endregion Messages



        private User _user = new User
        {
            UserId = 1,
            FirstName = "Johnny",
            LastName = "Tester"
        };

        private Organization _organization = new Organization
        {
            Id = 1,
            Name = "Manufactured Taste, Inc."
        };

    }
}
