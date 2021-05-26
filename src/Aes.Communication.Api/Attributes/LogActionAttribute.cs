using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Aes.Communication.Api.Logging;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace Aes.Communication.Api.Attributes
{
    public class LogActionAttribute : IActionFilter
    {
        private ILogger _logger;

        public LogActionAttribute(ILogger logger)
        {
            //_logger = new TestLogger();
            _logger = logger;
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            Log(context.RouteData, context.HttpContext);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        //todo: what needs to be logged...errors only? traffic?
        //todo: clean up
        private async Task Log(RouteData routeData, HttpContext context)
        {
            try
            {
                var identity = context.User.Identity as ClaimsIdentity;
                var clientId = "";
                if (identity != null)
                {
                    clientId = identity.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
                }


                var bodyStr = "";
                var req = context.Request;

                // Allows using several time the stream in ASP.Net Core
                req.EnableRewind();

                // Arguments: Stream, Encoding, detect encoding, buffer size 
                // AND, the most important: keep stream opened
                using (StreamReader reader
                    = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = reader.ReadToEnd();
                }

                // Rewind, so the core is not lost when it looks the body for the request
                req.Body.Position = 0;

                var body = await context.Request.GetRawBodyStringAsync();

                var parameters = new
                {
                    user = new { clientId },
                    connection = new
                    {
                        id = context.Connection.Id.ToString(),
                        localIpAddress = context.Connection.LocalIpAddress.ToString(),
                        localPort = context.Connection.LocalPort.ToString(),
                        remoteIpAddress = context.Connection.RemoteIpAddress.ToString(),
                        remotePort = context.Connection.RemotePort.ToString()
                    },
                    route = routeData.Values.ToDictionary(r => r.Key, r => r.Value.ToString(), StringComparer.OrdinalIgnoreCase),
                    host = context.Request.Host.ToString(),
                    headers = context.Request.Headers.Where(h=>!string.IsNullOrEmpty(h.Value)),
                    path = context.Request.Path.ToString(),
                    queryString = context.Request.QueryString.ToString(),
                    body
                };


                _logger.Log(new LogEntry
                {
                    LogDate = DateTime.Now,
                    Parameters = parameters,
                    Route = GetAbsoluteUri(context).ToString()
                });
            }
            catch (Exception ex)
            {
                var x = ex;
                //todo: notify somehow
                //do nothing
            }
        }


        private Uri GetAbsoluteUri(HttpContext context)
        {
            var request = context.Request;
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };

            return uriBuilder.Uri;
        }


    }

    public static class HttpRequestExtensions
    {
        public static async Task<string> GetRawBodyStringAsync(this HttpRequest request, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            var body = "";
            using (StreamReader reader = new StreamReader(request.Body, encoding))
            {
                //return await reader.ReadToEndAsync();
                try
                {
                    body = await reader.ReadToEndAsync();
                }
                catch (Exception ex)
                {
                    var x = ex;
                }
               
            }
            return body;
        }
    }

    
}
