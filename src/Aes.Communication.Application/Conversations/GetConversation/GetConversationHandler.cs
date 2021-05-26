using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aes.Communication.Application.Conversations.Repositories;
using Aes.Communication.Application.Exceptions;
using MediatR;

namespace Aes.Communication.Application.Conversations.GetConversation
{
    public class GetConversationHandler: IRequestHandler<GetConversationRequest,ConversationDto>
    {
        private IConversationReadOnlyRepository _repository;

        public GetConversationHandler(IConversationReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<ConversationDto> Handle(GetConversationRequest request, CancellationToken cancellationToken)
        {
            var conversation = _repository.GetConversation(request.ConversationId);
            if (conversation == null)
                throw new NotFoundException($"Conversation not found for id: {request.ConversationId}");

            return _repository.GetConversation(request.ConversationId);
        }
    }
}
