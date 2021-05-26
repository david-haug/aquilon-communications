using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Attributes;
using Aes.Communication.Api.Models;
using Aes.Communication.Application;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.AddMessageByConversationSubject;
using Aes.Communication.Application.Conversations.GetConversation;
using Aes.Communication.Application.Conversations.GetConversations;
using Aes.Communication.Domain.Messages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aes.Communication.Api.Messages
{
    [Route("messages")]
    [ApiController]
    [Produces("application/json")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    [EnableCors(CorsPolicies.Dev)]
    public class MessagesController: AppController
    {
        private readonly IMediator _mediator;
        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        ///// <summary>
        ///// Gets a conversation message by id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id}", Name = Routes.Messages.GetMessage)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<ConversationMessageDto>> Get([FromRoute]string id)
        //{
        //    //'var conversationreturn await _mediator.Send(new GetConversationRequest { ConversationId = id });
        //    ////TODO: testing:
        //    return new ConversationMessageDto
        //    {
        //        Body = ":)"
        //    };
        //}

        /// <summary>
        /// Add message to conversation identified by subject
        /// </summary>
        /// <param name="request">AddMessageBySubjectModel</param>
        /// <returns></returns>
        [HttpPost(Name = Routes.Messages.CreateMessage)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ConversationMessageDto>> Post([FromBody]AddMessageBySubjectModel request)
        {
            var result = await _mediator.Send(AddMessageBySubjectModel.Map(request));
            //return CreatedAtRoute(Routes.Messages.GetMessage, new { result.Id }, result);
            //TODO:return message route
            return CreatedAtRoute(Routes.Conversations.GetConversation, new { Id = result.ConversationId }, result);
        }
    }
}
