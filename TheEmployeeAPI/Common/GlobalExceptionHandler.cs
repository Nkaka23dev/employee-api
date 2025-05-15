using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using TheEmployeeAPI.Common;

namespace TheEmployeeAPI.Exceptions
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);
            var response = new ErrorResponse
            {
                Message = exception.Message
            };
            switch (exception)
            {
                case BadHttpRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Title = exception.GetType().Name;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Title = "Internal Server error";
                    break;

            }
            httpContext.Response.StatusCode = (int)response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
