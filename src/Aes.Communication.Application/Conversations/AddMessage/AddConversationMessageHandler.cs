using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using MediatR;

namespace Aes.Communication.Application.Conversations
{
    public class AddConversationMessageHandler : IRequestHandler<AddConversationMessageRequest, ConversationMessageDto>
    {
        private readonly AppUser _user;
        private readonly IConversationRepository _repository;

        public AddConversationMessageHandler(AppUser user, IConversationRepository repository)
        {
            _user = user;
            _repository = repository;
        }

        public async Task<ConversationMessageDto> Handle(AddConversationMessageRequest request, CancellationToken cancellationToken)
        {

            Conversation conversation = null;
            if(Guid.TryParse(request.ConversationId, out var guid))
                conversation = _repository.Get(guid);

            if (conversation == null)
                throw new NotFoundException($"Conversation not found for id: {request.ConversationId}");

            //todo authorize user...get parent entity of conversation, check orgs against users...use spec class

            var subject = request.Subject ?? conversation.Subject;
            var message = Message.Create(conversation, subject, request.Body, request.IsPublic, new User
            {
                UserId = request.User.UserId,
                FirstName = request.User.FirstName,
                LastName = request.User.LastName,
                Email = request.User.Email
            },request.Organization);

            message.Attachments = request.Attachments;

            conversation.AddMessage(message);

            //persist
            await _repository.Save(conversation);

            return ConversationMessageDto.Map(message);     
        }
    }
}
