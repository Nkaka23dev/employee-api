using TheEmployeeAPI;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();  
builder.Services.AddControllers(options => {
    options.Filters.Add<FluentValidationFilter>();
});
builder.Services.AddSingleton<ISystemClock, SystemClock>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppBbContext>(
    option => {
        option.UseSqlite(builder.Configuration.GetConnectionString("Default Connection"));
        // option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    } 
);
builder.Services.AddSwaggerDocument();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Employees API documentation";
    config.Title = "Employees API v1";
    config.Version = "v1";
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{   
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "Employees A PI";
        config.Path =  "/api";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}
using (var scope = app.Services.CreateScope()){
    var services = scope.ServiceProvider;
    SeedData.MigrateAndSeed(services);
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
public partial class Program {}