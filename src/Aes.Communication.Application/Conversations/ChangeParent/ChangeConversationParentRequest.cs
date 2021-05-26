using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;
using MediatR;

namespace Aes.Communication.Application.Conversations.ChangeParent
{
    public class ChangeConversationParentRequest:IRequest
    {
        public string ConversationId { get; set; }
        //public string ParentEntityId { get; set; }
        //public int ParentEntityType { get; set; }

        public EntityId Parent { get; set; }
    }
}
