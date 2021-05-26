using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using MediatR;

namespace Aes.Communication.Application.Conversations
{
    public class CreateConversationHandler:IRequestHandler<CreateConversationRequest,ConversationDto>
    {
        private AppUser _user;
        private IConversationRepository _repository;

        public CreateConversationHandler(AppUser user, IConversationRepository repository)
        {
            _user = user;
            _repository = repository;
        }

        public async Task<ConversationDto> Handle(CreateConversationRequest request, CancellationToken cancellationToken)
        {
            var conversation = Conversation.Create(
                request.Subject, 
                request.CreatedByUserId, 
                new ConversationTopic(request.Title,request.HeaderAttributes), 
                request.Parent,request.OrganizationId,request.CounterpartyId);

            //add messages if any
            if (request.Messages != null)
            {
                foreach (var msg in request.Messages)
                {
                    var subject = request.Subject ?? conversation.Subject;
                    var message = Message.Create(conversation, subject, msg.Body, msg.IsPublic, new User
                    {
                        UserId = msg.User.UserId,
                        FirstName = msg.User.FirstName,
                        LastName = msg.User.LastName
                    }, msg.Organization);
                    message.Attachments = msg.Attachments;
                    conversation.AddMessage(message);
                }
            }

            await _repository.Save(conversation);
            //return ConversationDto.Map(conversation,_user.UserId);
            return ConversationDto.Map(conversation);
        }
    }
}
