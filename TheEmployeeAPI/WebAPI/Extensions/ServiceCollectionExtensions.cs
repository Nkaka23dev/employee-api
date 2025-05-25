using System.Text;
using System.Text.Json;
using Core.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TheEmployeeAPI.Application.Authentication.MappingProfiles;
using TheEmployeeAPI.Application.Authentication.Services;
using TheEmployeeAPI.Application.Employees.MappingProfiles;
using TheEmployeeAPI.Application.Employees.Services;
using TheEmployeeAPI.Application.User.Services;
using TheEmployeeAPI.Common;
using TheEmployeeAPI.Domain.Contracts;
using TheEmployeeAPI.Persistance.Repositories;
using TheEmployeeAPI.Services;

namespace TheEmployeeAPI.WebAPI.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// CORS configuration registration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

            services.AddCors(options =>
            {
                options.AddPolicy("corsPolicy", builder =>
                {
                    builder.WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }

        /// <summary>
        /// JWT Secret confi and authorization registation
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (jwtSettings == null || string.IsNullOrWhiteSpace(jwtSettings.Key))
            {
                throw new InvalidOperationException("JWT secret key is not configured....");
            }
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(o =>
           {
               o.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = jwtSettings.ValidIssuer,
                   ValidAudience = jwtSettings.ValidAudience,
                   IssuerSigningKey = secretKey
               };

               o.Events = new JwtBearerEvents
               {
                   OnChallenge = context =>
                   {
                       context.HandleResponse();
                       var result = JsonSerializer.Serialize(new ErrorResponse
                       {
                           Title = "Unauthorized",
                           StatusCode = 401,
                           Message = "You are not authorized to access this resource, Please authenticate"
                       });

                       context.Response.StatusCode = 401;
                       context.Response.ContentType = "application/json";
                       return context.Response.WriteAsync(result);
                   },
                   OnForbidden = context =>
                   {
                       var result = JsonSerializer.Serialize(new
                       {
                           Title = "Forbidden",
                           StatusCode = 403,
                           message = "You do not have permission to access this resource, only Admins"
                       });
                       context.Response.StatusCode = 403;
                       context.Response.ContentType = "application/json";
                       return context.Response.WriteAsync(result);
                   }
               };
           });
        }
        /// <summary>
        /// Application Services Registration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
        /// <summary>
        /// Application Service Registration
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection repository)
        {
            repository.AddScoped<IUserRespository, UserRepository>();
            repository.AddScoped<IAuthRepository, AuthRepository>();
            repository.AddScoped<IEmployeeRepository, EmployeeRespository>();
            return repository;
        }
        /// <summary>
        /// Application mapping registration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddAutoMapper(typeof(MappingEmployee).Assembly);
            return services;
        }
    }


}
