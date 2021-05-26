using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using MediatR;

namespace Aes.Communication.Application.Conversations.AddMessageByConversationSubject
{
    public class AddMessageByConversationSubjectRequest : IRequest<ConversationMessageDto>
    {
        public EntityId ConversationSubject { get; set; }
        public string Body { get; set; }
        public bool IsPublic { get; set; }
        public User User { get; set; }
        public Organization Organization { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
        public EntityId MessageSubject { get; set; }
    }
}
