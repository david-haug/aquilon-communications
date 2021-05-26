using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;

namespace Aes.Communication.Infrastructure.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public string Body { get; set; }
        public bool IsPublic { get; set; }
        public User User { get; set; }
        public DateTime DateCreated { get; set; }
        public EntityId Subject { get; set; }

        public Organization Organization { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
        public IEnumerable<UserMessageRead> ReadByUsers { get; set; }
    }
}
