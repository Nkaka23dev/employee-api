using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SharedLibrary;

public class RestrictAccessMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var referrer = context.Request.Headers["Referrer"].FirstOrDefault();
        var result = JsonSerializer.Serialize(new ErrorResponse
        {
            Title = "Forbidden",
            StatusCode = 403,
            Message = "Access denied: All requests must be routed through the API Gateway for security and monitoring purposes."
        });
        if (string.IsNullOrEmpty(referrer))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(result);
            return;
        }
        else
        {
            await _next(context);
        }
    }
}
