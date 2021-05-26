using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Domain.Conversations
{
    public interface IConversationRepository
    {
        IEnumerable<Conversation> GetAll();
        Conversation Get(Guid id);
        Conversation GetBySubject(EntityId entityId);
        Task Save(Conversation conversation);

    }
}
