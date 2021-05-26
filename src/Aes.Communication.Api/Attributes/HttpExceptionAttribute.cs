using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aes.Communication.Api.Helpers;
using Aes.Communication.Api.Models;
using Aes.Communication.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aes.Communication.Api.Attributes
{
    /// <summary>
    /// Maps an exception to a relevant http status
    /// </summary>
    public class HttpExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                //400 BAD REQUEST (valdation errors)
                case ValidationException _:
                    context.Result = new ObjectResult(new ValidationErrorModel
                        {
                            Status = (int)HttpStatusCode.BadRequest,
                            Message = "Validation failed",
                            Errors = ((ValidationException)context.Exception).Errors
                                .Select(e=> new Application.Validation.ValidationError
                                {
                                    Code = e.ErrorCode,
                                    Message = e.ErrorMessage,
                                    Field = StringHelper.CreateCamelCaseObjectPath(e.PropertyName)
                                })
                        })
                        { StatusCode = (int)HttpStatusCode.BadRequest };
                    return;

                //400 BAD REQUEST
                case BadRequestException _:
                    var msg = context.Exception.Message ??
                              "The request contains malformed syntax or is missing required parameters.";
                    context.Result = new ObjectResult(new ErrorModel
                        {
                            Status = (int)HttpStatusCode.BadRequest,
                            Message = msg
                        })
                        { StatusCode = (int)HttpStatusCode.BadRequest };
                    return;

                //403 NOT AUTHORIZED
                case NotAuthorizedException _:
                    context.Result = new ObjectResult(new ErrorModel
                        {
                            Status = (int)HttpStatusCode.Forbidden,
                            Message = "The user does not have the required permissions to access the resource."
                        })
                        { StatusCode = (int)HttpStatusCode.Forbidden };
                    return;

                //404 NOT FOUND
                case NotFoundException _:
                    context.Result = new ObjectResult(new ErrorModel
                        {
                            Status = (int)HttpStatusCode.NotFound,
                            Message = context.Exception.Message
                        })
                        { StatusCode = (int)HttpStatusCode.NotFound };
                    return;
            }

            //500 - Unhandled
            context.Result = new ObjectResult(new ErrorModel
            {
                Status = (int)HttpStatusCode.InternalServerError,
                //DON'T RETURN MESSAGE TO CLIENT (security risk)
                //Message = context.Exception.Message
            })
            { StatusCode = (int)HttpStatusCode.InternalServerError };
        }

       
    }
}
