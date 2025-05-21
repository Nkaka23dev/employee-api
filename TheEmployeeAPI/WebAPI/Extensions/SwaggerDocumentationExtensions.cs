using Microsoft.OpenApi.Models;

namespace TheEmployeeAPI.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection SwaggerDocumentationExtensions(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TheEmployeeAPI.xml"));
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Employee API",
                    Version = "V1",
                    Description = "Employees Management systme"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token (no need to add 'Bearer' prefix)."
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

            return services;
        }
    }
}
