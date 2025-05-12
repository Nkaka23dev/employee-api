using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TheEmployeeAPI.Domain.Contracts;

namespace TheEmployeeAPI
{
    public static partial class ApplicationService
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("corsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }
        // public static void ConfigureIdentity(this IServiceCollection services){
        //     services.AddIdentityCore<IdentityUser>(
        //         u => {
        //             u.Password.RequiredLength = 6;
        //             u.User.RequireUniqueEmail = true;
        //         }
        //     ).AddEntityFrameworkStores<AppDbContext>()
        //     .AddDefaultTokenProviders();
        // }
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
                       var result = JsonSerializer.Serialize(new
                       {
                           message = "You are not authorized to access this resource, Please authenticate"
                       });

                       context.Response.StatusCode = 401;
                       context.Response.ContentType = "application/json";
                       return context.Response.WriteAsync(result);
                   }
               };
           });
        }
    }
}
