using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Attributes;
using Aes.Communication.Api.Models.Requests;
using Aes.Communication.Application.Conversations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aes.Communication.Api.ConversationMessages
{
    [Route("conversations/{conversationId}/messages")]
    [ApiController]
    [Produces("application/json")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    [EnableCors(CorsPolicies.Dev)]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class ConversationMessagesController:AppController
    {
        private readonly IMediator _mediator;

        public ConversationMessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Adds a message to a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="request">AddMessageModel</param>
        /// <returns></returns>
        [HttpPost(Name = Routes.ConversationMessages.CreateConversationMessage)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ConversationMessageDto>> Post([FromRoute]string conversationId, [FromBody]AddMessageModel request)
        {
            var result = await _mediator.Send(AddMessageModel.Map(request,conversationId));
            //return CreatedAtRoute(Routes.Messages.GetMessage, new { result.Id }, result);
            //todo: return message route
            return CreatedAtRoute(Routes.Conversations.GetConversation, new { Id = result.ConversationId }, result);
        }
    }
}
