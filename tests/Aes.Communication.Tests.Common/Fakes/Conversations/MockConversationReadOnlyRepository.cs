using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.GetConversations;
using Aes.Communication.Application.Conversations.Repositories;

namespace Aes.Communication.Tests.Common.Fakes.Conversations
{
    public class MockConversationReadOnlyRepository: IConversationReadOnlyRepository
    {
        public ConversationDto GetConversation(string id)
        {
            return null;
        }

        public IEnumerable<ConversationDto> GetAllConversations()
        {
            return null;
        }

        public IEnumerable<ConversationDto> GetConversations(GetConversationsRequest query)
        {
            throw new NotImplementedException();
        }
    }
}
