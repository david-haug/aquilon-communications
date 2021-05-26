using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using MediatR;

namespace Aes.Communication.Application.Conversations
{
    public class AddConversationMessageRequest : IRequest<ConversationMessageDto>
    {
        public string Body { get; set; }
        public bool IsPublic { get; set; }
        public string ConversationId { get; set; }
        public EntityId Subject { get; set; }
        public User User { get; set; }
        public Organization Organization { get; set; }

        public IEnumerable<Attachment> Attachments { get; set; }
    }
}
