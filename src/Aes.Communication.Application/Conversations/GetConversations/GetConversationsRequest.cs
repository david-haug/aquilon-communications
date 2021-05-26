using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;
using MediatR;

namespace Aes.Communication.Application.Conversations.GetConversations
{
    public class GetConversationsRequest: IRequest<QueryResult<ConversationDto>>
    {
        //public string SubjectId { get; set; }
        //public int? SubjectType { get; set; }
        //public string ParentId { get; set; }
        //public int? ParentType { get; set; }

        public EntityId Subject { get; set; }
        public EntityId Parent { get; set; }

        //todo: paging, sorting, etc
        //pagination
        //int? Limit { get; set; }
        //int? Offset { get; set; }
    }
}
