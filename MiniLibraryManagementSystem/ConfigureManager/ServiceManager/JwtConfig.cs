using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ConfigureManager.ServiceManager
{
    public static partial class ServiceManager
    {
        public static IServiceCollection JwtConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var _key = configuration["Jwt:Key"];
            var _issuer = configuration["Jwt:Issuer"];
            var _audience = configuration["Jwt:Audience"];

            var defaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = _issuer,
                            ValidAudience = _audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key))
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                return context.Response.WriteAsync("{\"message\":\"Authentication failed\"}");
                            },
                            OnChallenge = context =>
                            {
                                context.HandleResponse();
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                return context.Response.WriteAsync("{\"message\":\"Unauthorized access\"}");
                            },
                            OnForbidden = context =>
                            {
                                context.Response.StatusCode = 403;
                                context.Response.ContentType = "application/json";
                                return context.Response.WriteAsync("{\"message\":\"You do not have permission to access this resource\"}");
                            }
                        };
                    });
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = defaultPolicy; // all endpoints require authentication by default with allowed schemes
                // we can use default policy in controllers or actions with [Authorize] attribute
            });

            return services;
        }
    }
}
