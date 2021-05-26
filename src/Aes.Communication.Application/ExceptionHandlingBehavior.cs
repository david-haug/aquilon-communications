using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aes.Communication.Application.Exceptions;
using MediatR;

namespace Aes.Communication.Application
{
    /// <summary>
    /// Wraps domain exceptions into app-specific exceptions
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                //need to await result to allow exception to be caught within this behavior...
                var result = await next();
                return result;
            }
            //map domain exceptions to application layer exceptions
            catch (ArgumentException ex)
            {
                throw new BadRequestException(ex.ParamName);
            }
            catch (ApplicationException ex)
            {
                throw new BadRequestException(ex.Message);
            }


        }
    }
}
