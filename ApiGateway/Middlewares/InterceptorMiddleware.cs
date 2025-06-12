namespace ApiGateway.Middlewares;

public class InterceptorMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Headers["Referrer"] = "Api-Gateway";
        await next(context);
    }
}

