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

namespace Aes.Communication.Api.Messages.Read
{
    [Route("messages")]
    [ApiController]
    [Produces("application/json")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    [EnableCors(CorsPolicies.Dev)]
    public class MessagesReadController:AppController
    {
        private readonly IMediator _mediator;

        public MessagesReadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Marks all messages for the conversation as read
        /// </summary>
        /// <param name="MarkMessagesAsReadRequest">MarkMessagesAsReadRequest</param>
        /// <returns></returns>
        [HttpPost("read",Name = Routes.Messages.ReadMessages)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PostMessages([FromBody]MarkMessagesAsReadRequest request)
        {
            await _mediator.Send(request);
            return NoContent();
        }

    }
}
