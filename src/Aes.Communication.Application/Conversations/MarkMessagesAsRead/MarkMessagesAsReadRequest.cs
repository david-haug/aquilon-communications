using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.ValueObjects;
using MediatR;

namespace Aes.Communication.Application.Conversations.MarkMessagesAsRead
{
    public class MarkMessagesAsReadRequest:IRequest
    {
        public string ConversationId { get; set; }
        public User User { get; set; }
        public int OrganizationId { get; set; }
    }
}
