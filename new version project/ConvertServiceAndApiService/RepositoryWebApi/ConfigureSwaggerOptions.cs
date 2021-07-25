using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Tasks;

namespace RepositoryWebApi
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
       private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
             _provider = provider;
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var apiVersion = description.ApiVersion.ToString();
                options.SwaggerDoc(description.GroupName,
                    new OpenApiInfo
                    {

                        Version = apiVersion,
                        Title = $"Repository Api {apiVersion}",
                        Description = 
                        "A simple example ASP NET Core Web API.",
                        TermsOfService = new Uri("https://www.youtube.com/c/PlatinumTechTalks"),
                        //Contact = new OpenApiContact
                        //{
                        //    Name = "....",
                        //    Email = string.Empty,
                        //    Url = 
                        //        new Uri("https://....//")
                        //},
                        //License = new OpenApiLicense
                        //{
                        //    Name = "....",
                        //    Url = 
                        //        new Uri("https://....//")
                        //}
                    });
                options.AddSecurityDefinition($"AuthToken{apiVersion}",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Name = "Authorization",
                        Description = "Authorization token"
                    });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { 
                         new OpenApiSecurityScheme
                             {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = $"AuthToken {apiVersion}"
                             }},
                         new string[] {}
                    }
                });

                options.CustomOperationIds(ApiDescription =>
                ApiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
                ? methodInfo.Name
                : null);
            }
        }

    }
}
