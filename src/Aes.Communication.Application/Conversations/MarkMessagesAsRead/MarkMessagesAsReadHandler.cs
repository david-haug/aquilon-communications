using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aes.Communication.Application.Conversations.Repositories;
using Aes.Communication.Application.Events;
using Aes.Communication.Application.Exceptions;
using Aes.Communication.Domain;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using MediatR;

namespace Aes.Communication.Application.Conversations.MarkMessagesAsRead
{
    public class MarkMessagesAsReadHandler:IRequestHandler<MarkMessagesAsReadRequest>
    {
        private AppUser _user;
        private IConversationRepository _repository;
        private EventDispatcher _dispatcher;
        public MarkMessagesAsReadHandler(AppUser user, IConversationRepository repository, EventDispatcher dispatcher)
        {
            _user = user;
            _repository = repository;
            _dispatcher = dispatcher;
        }
        
        public Task<Unit> Handle(MarkMessagesAsReadRequest request, CancellationToken cancellationToken)
        {

            Conversation conversation = null;
            if (Guid.TryParse(request.ConversationId, out var guid))
                conversation = _repository.Get(guid);

            if(conversation == null)
                throw new NotFoundException($"Conversation not found for id: {request.ConversationId}");

            //will capture any exceptions for any of the messages and return at end
            var exceptions = new List<Exception>();

            //foreach (var id in request.MessageIds)
            //{
            //    if (Guid.TryParse(id, out guid))
            //    {
            //        var messageGuid = guid;
            //        var message = conversation.Messages.ToList().FirstOrDefault(m => m.Id == messageGuid);   
                    
            //        if(message == null)
            //            exceptions.Add(new NotFoundException($"Message not found for id: {id}"));
            //        try
            //        {
            //            conversation.MarkMessageAsRead(message, new User
            //            {
            //                UserId = request.User.UserId,
            //                FirstName = request.User.FirstName,
            //                LastName = request.User.LastName
            //            }, request.OrganizationId);
            //        }
            //        catch (Exception ex)
            //        {
            //            exceptions.Add(ex);
            //        }
                    
            //    }
            //}

            foreach (var message in conversation.Messages)
            {
                try
                {
                    conversation.MarkMessageAsRead(message, new User
                    {
                        UserId = request.User.UserId,
                        FirstName = request.User.FirstName,
                        LastName = request.User.LastName
                    }, request.OrganizationId);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            //todo: authorize
            //todo: handle exceptions...just throwing first for now
            if (exceptions.Any())
                throw exceptions[0];

            //foreach (var msg in messages)
            //{
            //    msg.MarkAsRead(_user.UserId);
            //    //todo only update if changed...update all at once
            //    _repository.Update(msg);

            //    //todo: do in decorator
            //    //_dispatcher.Dispatch(msg);
            //    DomainEvents.Dispatcher = _dispatcher;
            //    DomainEvents.Dispatch();

            //}

            _repository.Save(conversation);

            //todo: dispatch events

            return Unit.Task;
        }
    }
}
