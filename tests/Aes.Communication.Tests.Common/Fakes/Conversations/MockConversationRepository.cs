using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Tests.Common.Fakes.Conversations
{
    public class MockConversationRepository: IConversationRepository
    {
        private Conversation _conversation;
        public IEnumerable<Conversation> GetAll()
        {
            throw new NotImplementedException();
        }

        public Conversation Get(Guid id)
        {
            if(_conversation?.Id == id)
                return Conversation.Load(_conversation.Id, _conversation.Subject, _conversation.DateCreated,
                    _conversation.CreatedByUserId, _conversation.Messages, _conversation.Topic, _conversation.UserFlags,
                    _conversation.Parent,_conversation.OrganizationId,_conversation.CounterpartyId);

            return null;
        }

        public Conversation GetBySubject(EntityId entityId)
        {
            if(_conversation.Subject.Id == entityId.Id && _conversation.Subject.Type == entityId.Type)
                return Conversation.Load(_conversation.Id, _conversation.Subject, _conversation.DateCreated,
                    _conversation.CreatedByUserId, _conversation.Messages, _conversation.Topic, _conversation.UserFlags,
                    _conversation.Parent, _conversation.OrganizationId, _conversation.CounterpartyId);

            return null;
        }

        public Task Save(Conversation conversation)
        {
            _conversation = conversation;
            return Task.CompletedTask;
        }

        public Task SaveAsync(Conversation conversation)
        {
            throw new NotImplementedException();
        }
    }
}
