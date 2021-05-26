using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Infrastructure.Conversations;

namespace Aes.Communication.Infrastructure.Messages
{
    public class ConversationMessageJsonRepository:IMessageRepository
    {
        private ConversationJsonRepository _conversationRepository;

        public ConversationMessageJsonRepository(ConversationJsonRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public Message Get(Guid id)
        {
            return GetMessageFromConversations(id);
        }

        public void Add(Message message)
        {
            //get conversation

            //add message

            throw new NotImplementedException();
        }

        public void Update(Message message)
        {
            //get conversation
            var conversation = _conversationRepository.Get(message.ConversationId);
            
            var messages = conversation.Messages.Where(m=>m.Id!=message.Id).ToList();
            messages.Add(message);
            messages = messages.OrderBy(m => m.DateCreated).ToList();

            var updated = Conversation.Load(conversation.Id, conversation.Subject, conversation.DateCreated,
                conversation.CreatedByUserId, messages, conversation.Topic, conversation.UserFlags, conversation.Parent, conversation.OrganizationId,conversation.CounterpartyId);

            _conversationRepository.Save(updated);
        }

        private Message GetMessageFromConversations(Guid id)
        {
            var conversations = _conversationRepository.GetAll();
            return conversations.Select(
                conversation => conversation.Messages.FirstOrDefault(m => m.Id == id)).FirstOrDefault(message => message != null);
        }
    }
}
