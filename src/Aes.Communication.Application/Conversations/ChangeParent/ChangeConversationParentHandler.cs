using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aes.Communication.Application.Events;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using MediatR;

namespace Aes.Communication.Application.Conversations.ChangeParent
{
    public class ChangeConversationParentHandler:IRequestHandler<ChangeConversationParentRequest>
    {
        private AppUser _user;
        private IConversationRepository _repository;
        public ChangeConversationParentHandler(AppUser user, IConversationRepository repository)
        {
            _user = user;
            _repository = repository;
        }

        public Task<Unit> Handle(ChangeConversationParentRequest request, CancellationToken cancellationToken)
        {
            Conversation conversation = null;
            if (Guid.TryParse(request.ConversationId, out var guid))
                conversation = _repository.Get(guid);

            if (conversation == null)
                throw new NotFoundException($"Conversation not found for id: {request.ConversationId}");

            conversation.ChangeParent(request.Parent);
            _repository.Save(conversation);

            return Unit.Task;
        }
    }
}
