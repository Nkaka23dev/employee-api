using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using TheEmployeeAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Domain.Entities;
using TheEmployeeAPI.Persistance.Seed;
using TheEmployeeAPI.WebAPI.Extensions;
using TheEmployeeAPI.Infrastructure.DbContexts;
using TheEmployeeAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});
builder.Services.AddSingleton<ISystemClock, SystemClock>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(
    option =>
    {
        option.UseSqlite(builder.Configuration.GetConnectionString("Default Connection"));
    }
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigureCors();

builder.Services
    .AddApplicationServices()
    .AddRepositories()
    .AddMappingProfiles();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.SwaggerDocumentationExtensions();

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
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.MigrateAndSeed(services);
}
app.UseCors("corsPolicy");
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
public partial class Program { }