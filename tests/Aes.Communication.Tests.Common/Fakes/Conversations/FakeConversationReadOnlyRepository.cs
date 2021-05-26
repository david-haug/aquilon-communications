using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.GetConversations;
using Aes.Communication.Application.Conversations.Repositories;

namespace Aes.Communication.Tests.Common.Fakes.Conversations
{
    public class FakeConversationReadOnlyRepository: IConversationReadOnlyRepository
    {
        public ConversationDto GetConversation(string id)
        {
            return new ConversationDto {Id = Guid.Parse(id)};
        }

        public IEnumerable<ConversationDto> GetAllConversations()
        {
            return new List<ConversationDto>
            {
                new ConversationDto {Id = Guid.NewGuid()},
                new ConversationDto {Id = Guid.NewGuid()},
                new ConversationDto {Id = Guid.NewGuid()}
            };
        }

        public IEnumerable<ConversationDto> GetConversations(GetConversationsRequest query)
        {
            return new List<ConversationDto>
            {
                new ConversationDto {Id = Guid.NewGuid()},
                new ConversationDto {Id = Guid.NewGuid()},
                new ConversationDto {Id = Guid.NewGuid()}
            };
        }
    }
}
