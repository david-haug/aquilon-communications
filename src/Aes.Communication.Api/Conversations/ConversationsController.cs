using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Attributes;
using Aes.Communication.Api.Models;
using Aes.Communication.Application;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Application.Conversations.ChangeParent;
using Aes.Communication.Application.Conversations.UpdateTopic;
using Aes.Communication.Application.Conversations.GetConversation;
using Aes.Communication.Application.Conversations.GetConversations;
using Aes.Communication.Domain.Messages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aes.Communication.Api.Conversations
{
    [Route("conversations")]
    [ApiController]
    [Produces("application/json")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    [EnableCors(CorsPolicies.Dev)]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class ConversationsController:AppController
    {
        private readonly IMediator _mediator;
        public ConversationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all conversations matching supplied criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<QueryResult<ConversationDto>>> Get([FromQuery] GetConversationsModel request)
        {
            return await _mediator.Send(GetConversationsModel.Map(request));
        }

        /// <summary>
        /// Get conversation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = Routes.Conversations.GetConversation)]
        [ProducesResponseType(StatusCodes.Status200OK)]
       
        public async Task<ActionResult<ConversationDto>> Get([FromRoute]string id)
        {
            return await _mediator.Send(new GetConversationRequest { ConversationId = id });
        }

        /// <summary>
        /// Create new conversation
        /// </summary>
        /// <param name="request">CreateConversationModel</param>
        /// <returns></returns>
        [HttpPost(Name = Routes.Conversations.CreateConversation)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ConversationDto>> Post([FromBody]CreateConversationModel request)
        {
            var result = await _mediator.Send(CreateConversationModel.Map(request));
            return CreatedAtRoute(Routes.Conversations.GetConversation, new { result.Id }, result);
        }

        /// <summary>
        /// Updates a conversation topic and/or parent
        /// </summary>
        /// <param name="id">Conversation ID</param>
        /// <param name="request">UpdateConversationModel</param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = Routes.Conversations.UpdateConversation)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ConversationDto> Patch([FromRoute]string id, [FromBody]UpdateConversationModel model)
        public async Task<ActionResult> Patch([FromRoute]string id, [FromBody]UpdateConversationModel request)
        {
            //TODO: one handler? return Dto?...now this needs a test to determine if right requests are sent...
            if (request.Topic != null)
                await _mediator.Send(new UpdateConversationTopicRequest
                {
                    ConversationId = id,
                    Title = request.Topic.Title,
                    Attributes = request.Topic.Attributes
                });


            if (request.Parent != null)
                await _mediator.Send(new ChangeConversationParentRequest
                {
                    ConversationId = id,
                    Parent = EntityIdModel.Map(request.Parent)
                });

            return NoContent();
        }


    }
}
