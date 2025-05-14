namespace TheEmployeeAPI.WebAPI.Extensions;

public static class SwaggerMiddlewareExtensions
{
    public static void UseSwaggerDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
