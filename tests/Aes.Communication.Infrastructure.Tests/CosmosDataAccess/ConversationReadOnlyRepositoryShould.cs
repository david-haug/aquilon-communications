using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.GetConversations;
using Aes.Communication.Application.Conversations.Repositories;
using Aes.Communication.Infrastructure.Conversations;
using Aes.Communication.Infrastructure.CosmosDataAccess.Repositories;
using Xunit;

namespace Aes.Communication.Infrastructure.Tests.CosmosDataAccess
{
    public class ConversationReadOnlyRepositoryShould
    {
        private string _validId = Settings.Cosmos.ValidConversationId;
        private ConversationReadOnlyRepository _repository = new ConversationReadOnlyRepository(Settings.Cosmos.ConnectionString, 
            Settings.Cosmos.CommunicationsDatabase, Settings.Cosmos.ConversationsCollection);

        [Fact]
        public void ReturnConversationDtoGivenValidId()
        {
            var sut = _repository;

            var id = _validId;
            var actual = sut.GetConversation(id);
            Assert.Equal(id, actual.Id.ToString());
            Assert.IsType<ConversationDto>(actual);
        }

        [Fact]
        public void ReturnNullGivenInvalidId()
        {
            var sut = _repository;

            var id = "123";
            Assert.Null(sut.GetConversation(id));
        }

        [Fact]
        public void ReturnAllConversations()
        {
            var sut = _repository;
            var actual = sut.GetAllConversations();
            Assert.NotEmpty(actual);
        }

        [Fact]
        public void ReturnConversations()
        {
            var sut = _repository;
            var request = new GetConversationsRequest
            {
                Parent = new Domain.Messages.EntityId("12345", Domain.Messages.MessageEntityType.TieOut),
                Subject = new Domain.Messages.EntityId("1", Domain.Messages.MessageEntityType.Invoice)
            };
            var actual = sut.GetConversations(request);
            Assert.NotEmpty(actual);
        }


        [Fact]
        public void CreateSqlStatement()
        {
            var sut = _repository;

            var request = new GetConversationsRequest
            {
                Parent = new Domain.Messages.EntityId("12345", Domain.Messages.MessageEntityType.TieOut),
                Subject = new Domain.Messages.EntityId("9876", Domain.Messages.MessageEntityType.Invoice)
            };
            var expected = "select * from c where c.item.parent.type = 'tieout' and c.item.parent.id = '12345' and c.item.subject.type = 'invoice' and c.item.subject.id = '9876'";

            //var request = new GetConversationsRequest
            //{
            //    Parent = new Domain.Messages.EntityId("12345", Domain.Messages.MessageEntityType.TieOut),
            //};
            //var expected = "select * from c where c.parent.type = 'tieout' and c.parent.id = '12345'";

            //var request = new GetConversationsRequest
            //{
            //    Subject = new Domain.Messages.EntityId("9876", Domain.Messages.MessageEntityType.Invoice)
            //};
            //var expected = "select * from c where c.subject.type = 'invoice' and c.subject.id = '9876'";

            //var request = new GetConversationsRequest
            //{
            //    Subject = new Domain.Messages.EntityId("", Domain.Messages.MessageEntityType.Invoice)
            //};
            //var expected = "select * from c where c.subject.type = 'invoice'";

            var sql = sut.CreateSqlStatement(request);
            Assert.Equal(expected,sql);

        }
    }
}
