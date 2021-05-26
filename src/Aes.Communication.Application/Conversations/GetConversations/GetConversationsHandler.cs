using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aes.Communication.Application.Conversations.Repositories;
using MediatR;

namespace Aes.Communication.Application.Conversations.GetConversations
{
    public class GetConversationsHandler : IRequestHandler<GetConversationsRequest, QueryResult<ConversationDto>>
    {
        private IConversationReadOnlyRepository _repository;

        public GetConversationsHandler(IConversationReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult<ConversationDto>> Handle(GetConversationsRequest request, CancellationToken cancellationToken)
        {
            var conversations = _repository.GetConversations(request);
            return new QueryResult<ConversationDto> {Items = conversations, TotalCount = conversations.Count()};
        }
    }
}
