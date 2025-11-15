using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ConfigureManager.ServiceManager
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
            => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo()
                {
                    Title = "M.A. Monaem Khan",
                    Version = description.ApiVersion.ToString()
                });
            }
        }
    }
    public static partial class ServiceManager
    {
        public static IServiceCollection VersioningServices(this IServiceCollection services)
        {
            // --- API Versioning ---
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(2, 0);   // Default = v1.0
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;                   // Adds headers: api-supported-versions
            });

            // --- API Explorer for Swagger ---
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";   // v1, v2, etc.
                options.SubstituteApiVersionInUrl = true;
            });

            services.ConfigureOptions<ConfigureSwaggerOptions>();

            return services;
        }
    }
}
