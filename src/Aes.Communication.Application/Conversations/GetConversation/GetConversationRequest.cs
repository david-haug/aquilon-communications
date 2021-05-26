using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Conversations.Repositories;
using MediatR;

namespace Aes.Communication.Application.Conversations.GetConversation
{
    public class GetConversationRequest:IRequest<ConversationDto>
    {
        public string ConversationId { get; set; }

    }
}
