using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Attributes;
using Aes.Communication.Application.Conversations.ToggleUserFlag;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aes.Communication.Api.Conversations.Flags
{
    [Route("conversations/{conversationId}/flags")]
    [ApiController]
    [Produces("application/json")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [HttpException]
    [EnableCors(CorsPolicies.Dev)]
    [ServiceFilter(typeof(LogActionAttribute))]
    public class ConversationUserFlagsController:AppController
    {

        //private readonly IMediator _mediator;

        //public ConversationUserFlagsController(IMediator mediator)
        //{
        //    _mediator = mediator;
        //}

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> Post([FromRoute]string conversationId)
        //{
        //    await _mediator.Send(new ToggleConversationUserFlagRequest
        //    {
        //        ConversationId = conversationId,
        //        UserId = AppUser.UserId,
        //        FlaggedByUser = true
        //    });

        //    return Ok();
        //}

        //[HttpDelete]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //public async Task<ActionResult> Delete([FromRoute]string conversationId)
        //{
        //    await _mediator.Send(new ToggleConversationUserFlagRequest
        //    {
        //        ConversationId = conversationId,
        //        UserId = AppUser.UserId,
        //        FlaggedByUser = false
        //    });

        //    return NoContent();
        //}
    }
}
