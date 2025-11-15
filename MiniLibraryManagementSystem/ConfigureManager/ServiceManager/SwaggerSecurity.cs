using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ConfigureManager.ServiceManager
{
    public static partial class ServiceManager
    {
        public static IServiceCollection SwaggerSecurity(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Add JWT Bearer Authorization
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",                    // Standard header name
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",                         // Must be "bearer"
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter Your Token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            return services;
        }
    }
}
