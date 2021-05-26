using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Attributes;
using Aes.Communication.Application.Conversations.MarkMessagesAsRead;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aes.Communication.Api.Conversations.MarkAsRead
{
    [Route("conversations/{id}/markasread")]
    [ApiController]
    [Produces("application/json")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    [EnableCors(CorsPolicies.Dev)]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class ConversationMarkAsReadController : AppController
    {
        private readonly IMediator _mediator;

        public ConversationMarkAsReadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Marks all messages for the conversation as read
        /// </summary>
        /// <param name="id">Conversation ID</param>
        /// <param name="request">MarkAsReadModel</param>
        /// <returns></returns>
        [HttpPost(Name = Routes.Conversations.ReadMessages)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Post([FromRoute]string id, [FromBody]MarkAsReadModel request)
        {
            await _mediator.Send(new MarkMessagesAsReadRequest
            {
                ConversationId = id,
                User = request.User,
                OrganizationId = request.OrganizationId

            });
            return NoContent();
        }
    }
}
