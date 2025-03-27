using TheEmployeeAPI;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TheEmployeeAPI.xml"));
});
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();  
builder.Services.AddControllers(options => {
    options.Filters.Add<FluentValidationFilter>();
});
builder.Services.AddSingleton<ISystemClock, SystemClock>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(
    option => {
        option.UseSqlite(builder.Configuration.GetConnectionString("Default Connection"));
        // option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    } 
);
builder.Services.AddSwaggerDocument();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{   
    app.UseSwagger();
       app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API v1");
        c.RoutePrefix = string.Empty; 
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