using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Application.Conversations
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public EntityId Subject { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedByUserId { get; set; }
        public IEnumerable<ConversationMessageDto> Messages { get; set; } = new List<ConversationMessageDto>();
        public EntityId Parent { get; set; }
        public ConversationTopic Topic { get; set; }
        public int OrganizationId { get; set; }
        public int CounterpartyId { get; set; }

        public int UnreadMessagesOrganization { get; private set; }
        public int UnreadMessagesCounterparty { get; private set; }

        //public ConversationUserFlagDto UserFlag { get; set; }

        //public static ConversationDto Map(Conversation conversation, int appUserId)
        //{
        //    var messages = new List<ConversationMessageDto>();
        //    foreach (var message in conversation.Messages)
        //        messages.Add(ConversationMessageDto.Map(message));

        //    return new ConversationDto
        //    {
        //        Id = conversation.Id,
        //        Subject = conversation.Subject,
        //        DateCreated = conversation.DateCreated,
        //        CreatedByUserId = conversation.CreatedByUserId,
        //        Messages = messages,
        //        Title = conversation.Topic.Title,
        //        HeaderAttributes = conversation.Topic.Attributes,
        //        UserFlag = conversation.UserFlags.Select(f=> new ConversationUserFlagDto
        //        {
        //            UserId = f.UserId,
        //            DateCreated = f.DateCreated
        //        }) .FirstOrDefault(f => f.UserId==appUserId)
        //    };
        //}

        public static ConversationDto Map(Conversation conversation)
        {
            if (conversation == null)
            {
                return null;
            }

            var messages = new List<ConversationMessageDto>();
            foreach (var message in conversation.Messages)
            {
                messages.Add(ConversationMessageDto.Map(message));
            }

            return new ConversationDto
            {
                Id = conversation.Id,
                Subject = conversation.Subject,
                DateCreated = conversation.DateCreated,
                CreatedByUserId = conversation.CreatedByUserId,
                Messages = messages,
                Parent = conversation.Parent,
                Topic = conversation.Topic,
                OrganizationId = conversation.OrganizationId,
                CounterpartyId = conversation.CounterpartyId,
                UnreadMessagesCounterparty = conversation.GetUnreadMessageCountByOrganization(conversation.CounterpartyId),
                UnreadMessagesOrganization = conversation.GetUnreadMessageCountByOrganization(conversation.OrganizationId)
            };
        }

    }
}
