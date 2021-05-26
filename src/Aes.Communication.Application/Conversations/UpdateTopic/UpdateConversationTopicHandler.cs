using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Domain.Conversations;
using MediatR;

namespace Aes.Communication.Application.Conversations.UpdateTopic
{
    public class UpdateConversationTopicHandler:IRequestHandler<UpdateConversationTopicRequest,ConversationDto>
    {
        private AppUser _user;
        private IConversationRepository _repository;
        public UpdateConversationTopicHandler(AppUser user, IConversationRepository repository)
        {
            _user = user;
            _repository = repository;
        }

        public async Task<ConversationDto> Handle(UpdateConversationTopicRequest request, CancellationToken cancellationToken)
        {
            var conversation = _repository.Get(Guid.Parse(request.ConversationId));
            if (conversation == null)
                throw new NotFoundException($"Conversation not found for id: {request.ConversationId}");

            conversation.UpdateTopic(new ConversationTopic(request.Title, request.Attributes));
            _repository.Save(conversation);
            //return ConversationDto.Map(conversation, _user.UserId);
            return ConversationDto.Map(conversation);
        }
    }
}
