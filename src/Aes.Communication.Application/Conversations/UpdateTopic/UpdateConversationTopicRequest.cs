using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Aes.Communication.Application.Conversations.UpdateTopic
{
    public class UpdateConversationTopicRequest:IRequest<ConversationDto>
    {
        public string ConversationId { get; set; }
        public string Title { get; set; }
        public Dictionary<string,string> Attributes { get; set; }
    }
}
