using System;
using MediatR;

namespace Aes.Communication.Application.Conversations.ToggleUserFlag
{
    public class ToggleConversationUserFlagRequest: IRequest
    {
        public string ConversationId { get; set; }
        public int UserId { get; set; }
        public bool FlaggedByUser { get; set; }
    }
}
