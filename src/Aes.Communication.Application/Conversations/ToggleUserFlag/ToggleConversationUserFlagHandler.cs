using System;
using System.Threading;
using System.Threading.Tasks;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Domain.Conversations;
using MediatR;

namespace Aes.Communication.Application.Conversations.ToggleUserFlag
{
    public class ToggleConversationUserFlagHandler : IRequestHandler<ToggleConversationUserFlagRequest>
    {
        private AppUser _user;
        private IConversationRepository _repository;
        public ToggleConversationUserFlagHandler(AppUser user, IConversationRepository repository)
        {
            _user = user;
            _repository = repository;
        }

        public Task<Unit> Handle(ToggleConversationUserFlagRequest request, CancellationToken cancellationToken)
        {
            Conversation conversation = null;
            if (Guid.TryParse(request.ConversationId, out var guid))
                conversation = _repository.Get(guid);

            if (conversation == null)
                throw new NotFoundException($"Conversation not found for id: {request.ConversationId}");

            //todo authorize user...get parent entity of conversation, check orgs against users...use spec class

            if (request.FlaggedByUser)
            {
                conversation.AddUserFlag(request.UserId);
            }
            else
            {
                conversation.RemoveUserFlag(request.UserId);
            }

            _repository.Save(conversation);

            return Unit.Task;
        }
    }
}
