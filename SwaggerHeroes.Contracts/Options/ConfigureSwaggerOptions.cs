using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SwaggerHeroes.Core.SwaggerOptions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerHeroes.Core.Options
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;
        private readonly ApiSpecificationOptions apiSpecificationOptions;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            ApiSpecificationOptions apiSpecificationOptions
        )
        {
            this.provider = provider;
            this.apiSpecificationOptions = apiSpecificationOptions;
        }

        public void Configure(SwaggerGenOptions options)
        {
            // add swagger document for every API version discovered
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = apiSpecificationOptions.Title,
                Version = description.ApiVersion.ToString(),
                Contact = new OpenApiContact()
                {
                    Name = apiSpecificationOptions.Author,
                    Url = new Uri(apiSpecificationOptions.Website)
                },
                Description = apiSpecificationOptions.Description
            };

            if (description.IsDeprecated)
            {
                info.Description += " (This API version has been deprecated)";
            }

            return info;
        }
    }
}
