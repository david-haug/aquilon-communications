using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using Xunit;

namespace Aes.Communication.Domain.Tests.Messages
{
    public class MessageShould
    {
        [Fact]
        public void LoadGivenValidArguments()
        {
            //arrange
            var id = Guid.NewGuid();
            var conversationGuid = Guid.NewGuid();
            var entityId = new EntityId("1", MessageEntityType.Invoice);
            var body = "body";
            var isPublic = false;
            var date = DateTime.Now;
            //act
            var sut = Message.Load(id, conversationGuid, entityId, body, isPublic, _user, date, null,_organization);

            //assert
            Assert.Equal(id, sut.Id);
            Assert.Equal(body, sut.Body);
            Assert.Equal(isPublic, sut.IsPublic);
            Assert.Equal(_user.UserId, sut.User.UserId);
            Assert.Equal(date, sut.DateCreated);
            Assert.Equal(_organization.Id, sut.Organization.Id);
        }

        [Fact]
        public void ThrowArgumentExceptionGivenEmptyBody()
        {
            //arrange
            var id = Guid.NewGuid();
            var conversationGuid = Guid.NewGuid();
            var body = "";
            var isPublic = false;
            var userId = 1;
            var date = DateTime.Now;
            var entityId = new EntityId("1", MessageEntityType.Invoice);

            //act and assert
            Assert.Throws<ArgumentException>(() => Message.Load(id, conversationGuid, entityId, body, isPublic, _user, date, null, _organization));
        }

        [Fact]
        public void ThrowArgumentExceptionGivenNoCreatedByUserId()
        {
            //arrange
            var id = Guid.NewGuid();
            var conversationGuid = Guid.NewGuid();
            var body = "body";
            var isPublic = false;
            var date = DateTime.Now;
            var entityId = new EntityId("1", MessageEntityType.Invoice);
            var badUser = new User();
            //act and assert
            Assert.Throws<ArgumentException>(() => Message.Load(id, conversationGuid, entityId, body, isPublic, badUser, date, null, _organization));
        }
        [Fact]
        public void ThrowArgumentExceptionGivenInvalidSubject()
        {
            //arrange
            var id = Guid.NewGuid();
            var conversationGuid = Guid.NewGuid();
            var body = "";
            var isPublic = false;
            var userId = 1;
            var date = DateTime.Now;
            var entityId = new EntityId("", MessageEntityType.Invoice);

            //act and assert
            Assert.Throws<ArgumentException>(() => Message.Load(id, conversationGuid, entityId, body, isPublic, _user, date, null, _organization));
        }

        [Fact]
        public void ThrowArgumentExceptionGivenNullOrganization()
        {
            //arrange
            var id = Guid.NewGuid();
            var conversationGuid = Guid.NewGuid();
            var body = "body";
            var isPublic = false;
            var date = DateTime.Now;
            var entityId = new EntityId("1", MessageEntityType.Invoice);

            //act and assert
            Assert.Throws<ArgumentException>(() => Message.Load(id, conversationGuid, entityId, body, isPublic, _user, date, null, null));
        }

        [Fact]
        public void ThrowArgumentExceptionWhenOrganizationIdIsZero()
        {
            //arrange
            var id = Guid.NewGuid();
            var conversationGuid = Guid.NewGuid();
            var body = "body";
            var isPublic = false;
            var date = DateTime.Now;
            var entityId = new EntityId("1", MessageEntityType.Invoice);

            //act and assert
            Assert.Throws<ArgumentException>(() => Message.Load(id, conversationGuid, entityId, body, isPublic, _user, date, null, new Organization()));
        }

        //[Fact]
        //public void NotAddToReadByUsersWhenExistsForUser()
        //{
        //    //arrange
        //    var id = Guid.NewGuid();
        //    var conversationGuid = Guid.NewGuid();
        //    var body = "body";
        //    var isPublic = false;
        //    var userId = 1;
        //    var date = DateTime.Now;
        //    var entityId = new EntityId("1", MessageEntityType.Invoice);
        //    var readBy = new List<UserMessageRead> {new UserMessageRead(2, DateTime.Now)};

        //    var sut = Message.Load(id, conversationGuid, entityId, body, isPublic, userId, date, readBy);
        //    sut.MarkAsRead(2);
        //    Assert.Single(sut.ReadByUsers);
        //}

        //[Fact]
        //public void NotAddMessageCreatorToReadByUsers()
        //{
        //    //arrange
        //    var id = Guid.NewGuid();
        //    var conversationGuid = Guid.NewGuid();
        //    var body = "body";
        //    var isPublic = false;
        //    var userId = 1;
        //    var date = DateTime.Now;
        //    var entityId = new EntityId("1", MessageEntityType.Invoice);
        //    var readBy = new List<UserMessageRead> { new UserMessageRead(2, DateTime.Now) };

        //    var sut = Message.Load(id, conversationGuid, entityId, body, isPublic, userId, date, readBy);
        //    sut.MarkAsRead(userId);
        //    Assert.Single(sut.ReadByUsers);
        //}

        //[Fact]
        //public void AddMarkedAsReadEvent()
        //{
        //    //arrange
        //    var id = Guid.NewGuid();
        //    var conversationGuid = Guid.NewGuid();
        //    var body = "body";
        //    var isPublic = false;
        //    var userId = 1;
        //    var date = DateTime.Now;
        //    var entityId = new EntityId("1", MessageEntityType.Invoice);
        //    var readBy = new List<UserMessageRead> { new UserMessageRead(2, DateTime.Now) };

        //    var sut = Message.Load(id, conversationGuid, entityId, body, isPublic, userId, date, readBy);
        //    sut.MarkAsRead(3);

        //    Assert.NotEmpty(sut.Events);
        //    Assert.IsType<MessageMarkedAsRead>(sut.Events.ToList().FirstOrDefault());
        //}

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
