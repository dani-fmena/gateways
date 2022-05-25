using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using gateway.domain;

namespace gateway.api.Bootstrap
{
    /// <summary>
    /// Make the registration for open api (swagger) documentation 
    /// </summary>
    /// <remarks>This are extension method for IServiceCollection</remarks>
    public static class ApiDocRegistration
    {
        /// <summary>
        /// Register the documentation service
        /// </summary>
        /// <param name="services">Service collection instance</param>
        public static void AddApiDocumentation(this IServiceCollection services)
        {
            services.AddApiVersioning();
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerCustomConfig>();
            services.AddSwaggerGen();
        }
    }

    /// <summary>
    /// Define the custom configuration to be used with swagger 
    /// </summary>
    public class SwaggerCustomConfig : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public SwaggerCustomConfig(IApiVersionDescriptionProvider provider) => this._provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            // Enable Swagger Annotation | https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#swashbuckleaspnetcoreannotations
            // options.EnableAnnotations();
            options.DocumentFilter<ApplyTagDescriptions>();
            
            // Custom Tag Names Strategy 
            options.DocInclusionPredicate((name, api) => true);
            options.TagActionsBy(description =>
            {
                // ApiExplorerSettings.GroupName was given
                if (description.GroupName != null && !Regex.IsMatch(description.GroupName, @"^v\d+$", RegexOptions.IgnoreCase)) {
                    return new [] { description.GroupName };
                }

                // ApiExplorerSettings.GroupName was NOT given, so using ControllerName
                var controllerActionDescriptor = description.ActionDescriptor as ControllerActionDescriptor;
                if (controllerActionDescriptor != null) { return new [] { controllerActionDescriptor.ControllerName }; }

                // default case, should not be reached
                return new[] { "" };
            });
            
            // Include XML comments file (kernel assembly) for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPathKernel = Path.Combine(AppContext.BaseDirectory, xmlFile);
            
            // Include XML comments file (cosmos assembly) for the Swagger JSON and UI.
            var tPath = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName != null && a.FullName.Contains("gateway.domain"))
                .ToArray()[0]
                .Location
                .Replace("gateway.domain.dll", "");
            var xmlPathCosmos = Path.Combine(tPath, "gateway.domain.xml");
            
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                // General info
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"Gateways {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = "Devices management system",
                    
                    TermsOfService = new Uri("https://example.com/terms"),

                    Contact = new OpenApiContact
                    {
                        Name = "Admin",
                        Email = "support@gateways.musala.com",
                        Url = new Uri("https://www.gateways.musala.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                
                // Enabling XML comments for extended OpenApi information
                options.IncludeXmlComments(xmlPathKernel);
                options.IncludeXmlComments(xmlPathCosmos);
            }
        }

        /// <summary>
        /// Custom Swagger document filter. Defines the descriptions for all the declared endpoints Tags
        /// </summary>
        private class ApplyTagDescriptions : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                swaggerDoc.Tags = new List<OpenApiTag>
                {
                    // staff
                    new OpenApiTag { Name = EndpointTags.TgGateways, Description = "gateways managements endpoints" },
                    new OpenApiTag { Name = EndpointTags.TgPeripherals, Description = "peripherals managements endpoints" },
                };
            }
        }
    }
}