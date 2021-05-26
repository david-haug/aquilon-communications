using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.Messages;
using MediatR;

namespace Aes.Communication.Application.Conversations
{
    public class CreateConversationRequest : IRequest<ConversationDto>
    {
        public string Title { get; set; }
        public IDictionary<string,string> HeaderAttributes { get; set; }
        public EntityId Subject { get; set; }
        public EntityId Parent { get; set; }

        public int CreatedByUserId { get; set; }
        public int OrganizationId { get; set; }
        public int CounterpartyId { get; set; }
        public IEnumerable<AddConversationMessageRequest> Messages { get; set; }
    }
}
