using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using Aes.Communication.Infrastructure.Conversations;
using Aes.Communication.Tests.Common.Fakes.Users;
using Xunit;

namespace Aes.Communication.Infrastructure.Tests.Conversations
{
    public class ConversationJsonRepositoryShould
    {
        private string _file = Settings.Json.ConversationsFilePath;
        private string _testConversationGuid = Settings.Json.ValidConversationId;

        [Fact]
        public void SaveNewConversation()
        {
            var conversation = Conversation.Create(new EntityId("1", MessageEntityType.Dispute), 1, new ConversationTopic("title", new Dictionary<string,string>
            {
                {"Field1","Value1"},{"Field2","Value2"}      
            }), new EntityId("12345", MessageEntityType.TieOut),1,2);
            var sut = new ConversationJsonRepository(_file);
            sut.Save(conversation);
            var actual = sut.Get(conversation.Id).Id;
            Assert.Equal(conversation.Id, actual);
        }

        [Fact]
        public void SaveExistingConversation()
        {
            var id = Guid.Parse(_testConversationGuid);
            var sut = new ConversationJsonRepository(_file);
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

            sut.Save(conversation);

            var actual = sut.Get(id).Messages.Count;
            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public void ReturnConversationWithAttributesWhenExists()
        {
            var id = Guid.Parse(_testConversationGuid);
            var sut = new ConversationJsonRepository(_file);
            var conversation = sut.Get(id);
            var actual = conversation.Topic.Attributes.Count;
            Assert.Equal(2, actual);
        }

        [Fact]
        public void SaveUserFlagsWhenUpdatingConversation()
        {
            var conversation = Conversation.Create(
                new EntityId("1", MessageEntityType.Dispute), 
                1, new ConversationTopic("title", new Dictionary<string, string>
                {
                    {"Field1","Value1"},{"Field2","Value2"}
                }), new EntityId("12345", MessageEntityType.TieOut),1,2);
            var sut = new ConversationJsonRepository(_file);
            sut.Save(conversation);

            //now update with user flags
            conversation.AddUserFlag(1);
            conversation.AddUserFlag(2);
            var expected = conversation.UserFlags.Count;
            sut.Save(conversation);

            var actual = sut.Get(conversation.Id).UserFlags.Count;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SaveAttachmentsWhenUpdatingConversation()
        {
            var conversation = Conversation.Create(
                new EntityId("1", MessageEntityType.Dispute),
                1, new ConversationTopic("title", new Dictionary<string, string>
                {
                    {"Field1","Value1"},{"Field2","Value2"}
                }), new EntityId("12345", MessageEntityType.TieOut), 1, 2);
            var sut = new ConversationJsonRepository(_file);
            sut.Save(conversation);

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
            sut.Save(conversation);


            var saved = sut.Get(conversation.Id);
            var savedMsg = saved.Messages.FirstOrDefault(m=>m.Id == msg.Id);

            Assert.Equal(expected, savedMsg.Attachments.Count());
        }



    }
}
