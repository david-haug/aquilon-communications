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

namespace Aes.Communication.Application.Conversations.AddMessageByConversationSubject
{
    public class AddMessageByConversationSubjectHandler : IRequestHandler<AddMessageByConversationSubjectRequest, ConversationMessageDto>
    {
        private readonly AppUser _user;
        private readonly IConversationRepository _repository;

        public AddMessageByConversationSubjectHandler(AppUser user, IConversationRepository repository)
        {
            _user = user;
            _repository = repository;
        }

        public async Task<ConversationMessageDto> Handle(AddMessageByConversationSubjectRequest request, CancellationToken cancellationToken)
        {
            if (request.ConversationSubject == null)
                throw new BadRequestException("Conversation subject is required");

            Conversation conversation = null;
            conversation = _repository.GetBySubject(request.ConversationSubject);

            if (conversation == null)
                throw new NotFoundException($"Conversation not found for subject type: {request.ConversationSubject.Type} and id: {request.ConversationSubject.Id}");

            var msgSubject = request.MessageSubject ?? conversation.Subject;
            var message = Message.Create(conversation, msgSubject, request.Body, request.IsPublic, new User
            {
                UserId = request.User.UserId,
                FirstName = request.User.FirstName,
                LastName = request.User.LastName,
                Email = request.User.Email,
            }, request.Organization);

            message.Attachments = request.Attachments;
            conversation.AddMessage(message);

            //persist
            await _repository.Save(conversation);
            return ConversationMessageDto.Map(message);
        }
    }
}
