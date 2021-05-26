using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Tests.Common.Fakes.Conversations
{
    public class StubConversationRepository: IConversationRepository
    {
        public IEnumerable<Conversation> GetAll()
        {
            throw new NotImplementedException();
        }

        public Conversation Get(Guid id)
        {
            return new Conversation();
        }

        public Conversation GetBySubject(EntityId entityId)
        {
            return new Conversation();
        }

        public Task Save(Conversation conversation)
        {
            //do nothing
            return Task.CompletedTask;
        }

        public Task SaveAsync(Conversation conversation)
        {
            throw new NotImplementedException();
        }
    }
}
