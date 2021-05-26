using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Application;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.GetConversations;
using Aes.Communication.Application.Conversations.Repositories;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Infrastructure.Conversations
{
    public class ConversationReadOnlyJsonRepository:IConversationReadOnlyRepository
    {
        private ConversationJsonRepository _conversationJsonRepository;
        public AppUser _appUser;

        public ConversationReadOnlyJsonRepository(string file, AppUser appUser)
        {
            _conversationJsonRepository = new ConversationJsonRepository(file);
            _appUser = appUser;
        }

        public ConversationDto GetConversation(string id)
        {
            var valid = Guid.TryParse(id,out var guid);
            if (!valid) return null;

            var conversation = _conversationJsonRepository.Get(guid);
            //return ConversationDto.Map(conversation, _appUser.UserId);
            return ConversationDto.Map(conversation);
        }

        public IEnumerable<ConversationDto> GetAllConversations()
        {
            var conversations = _conversationJsonRepository.GetAll();
            //return conversations.Select(c => ConversationDto.Map(c,_appUser.UserId)).ToList();
            return conversations.Select(ConversationDto.Map).ToList();
        }

        public IEnumerable<ConversationDto> GetConversations(GetConversationsRequest query)
        {
            var conversations = _conversationJsonRepository.GetAll().ToList();
            var dtos = conversations.Select(ConversationDto.Map);

            //if (!string.IsNullOrWhiteSpace(query.ParentId))
            //    dtos = dtos.Where(c => c.Parent?.Id == query.ParentId);
            //if (query.ParentType.HasValue)
            //    dtos = dtos.Where(c => c.Parent?.EntityType == (MessageEntityType)query.ParentType);

            //if (!string.IsNullOrWhiteSpace(query.SubjectId))
            //    dtos = dtos.Where(c => c.Subject?.Id == query.SubjectId);
            //if (query.SubjectType.HasValue)
            //    dtos = dtos.Where(c => c.Subject?.EntityType == (MessageEntityType)query.SubjectType);

            if (query.Parent != null)
            {
                dtos = dtos.Where(c => c.Parent?.Id == query.Parent.Id 
                                       && c.Parent?.Type == query.Parent.Type);
            }
            if (query.Subject != null)
            {
                dtos = dtos.Where(c => c.Subject?.Id == query.Subject.Id
                                       && c.Subject?.Type == query.Subject.Type);
            }


            return dtos.ToList();
        }
    }
}
