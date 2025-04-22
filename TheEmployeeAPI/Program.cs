using TheEmployeeAPI;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using TheEmployeeAPI.Exceptions;
using Microsoft.OpenApi.Models;
using TheEmployeeAPI.Services.User;
using TheEmployeeAPI.Services;
using TheEmployeeAPI.Infrastructure.MappingProfile;
using TheEmployeeAPI.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using TheEmployeeAPI.Services.Auth;
using TheEmployeeAPI.Services.Employees;
using TheEmployeeAPI.Infrastructure.Context;

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
builder.Services.AddDbContext<AppDbContext>(
    option => {
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


builder.Services.AddScoped<IUserServices, UserService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigureCors();
 

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddSwaggerDocument();
builder.Services.AddSwaggerGen(options =>
{   
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TheEmployeeAPI.xml"));
    options.SwaggerDoc("v1",
     new OpenApiInfo 
     {
        Title = "Employee API",
        Version = "V1", 
        Description="Provides basic employee management features"
     });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please Enter a valid Token in the following format: {Your token here} do not add word Bearer before it."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>() 
        }
    });

    
});

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
app.UseCors("corsPolicy");
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
public partial class Program {}