using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartLibrary.BLL.Exceptions;

namespace SmartLibrary.API.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorMiddleware(RequestDelegate next, ILoggerFactory factory)
        {
            _next = next;
            _logger = factory.CreateLogger("errors");
        }

        /// <summary>
        /// Just basically runs the next piece of code with a try block around it and handles in case of any exceptions bubbling up here
        /// </summary>
        /// <param name="context">Context of the current HTTP pipeline</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (HttpException ex)
            {
                _logger.LogError(ex, ex.Message + "-" + ex.StackTrace);
                context.Response.StatusCode = ex.StatusCode;
            }
            catch (ApiException ex)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in ex.Error)
                {
                    stringBuilder.AppendLine(error.error);
                }
                _logger.LogError(ex, stringBuilder.ToString() + "-" + ex.StackTrace);
                context.Response.StatusCode = 500;
                var result = JsonConvert.SerializeObject(new { error = ex.Error });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message + "-" + ex.StackTrace);
                context.Response.StatusCode = 500;
            }
        }
    }
}
