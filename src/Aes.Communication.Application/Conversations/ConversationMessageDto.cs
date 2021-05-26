using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;

namespace Aes.Communication.Application.Conversations
{
    public class ConversationMessageDto
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public EntityId Subject { get; set; }
        public string Body { get; set; }
        public bool IsPublic { get; set; }
        public User User { get; set; }
        public Organization Organization { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
        public IEnumerable<UserMessageRead> ReadByUsers { get; set; }
        public bool IsUnread { get; private set; }
        public static ConversationMessageDto Map(Message message)
        {
            return new ConversationMessageDto
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                Subject = message.Subject,
                Body = message.Body,
                IsPublic = message.IsPublic,
                User = message.User,
                DateCreated = message.DateCreated,
                Organization = message.Organization,
                Attachments = message.Attachments,
                ReadByUsers = message.ReadByUsers,
                IsUnread = !message.ReadByUsers.Any()
            };
        }
    }
}
