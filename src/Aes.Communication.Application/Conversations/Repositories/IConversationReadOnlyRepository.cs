using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aes.Communication.Application.Conversations.GetConversations;

namespace Aes.Communication.Application.Conversations.Repositories
{
    public interface IConversationReadOnlyRepository
    {
        ConversationDto GetConversation(string id);
        IEnumerable<ConversationDto> GetAllConversations();

        IEnumerable<ConversationDto> GetConversations(GetConversationsRequest query);

        //Task<IEnumerable<ConversationDto>> GetConversationsAsync(GetConversationsRequest request);
    }
}
