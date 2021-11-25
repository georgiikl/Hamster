using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

using Hamster.Controllers;

namespace Hamster.Web.Extensions
{
    internal static class SwaggerExtensions
    {
        internal static IServiceCollection AddSwaggerGenerator(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddVersionInformation();
                //c.AddJwtAuthentication();
                c.AddComments();
            });
            return services;
        }

        // todo: вынести в константы
        private static void AddVersionInformation(this SwaggerGenOptions options)
        {
            //var descriptionFilePath = GetFullFilePath("PRACTIC_API.md");
            //var description = File.ReadAllText(descriptionFilePath);
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Hamster API",
                //Description = description
            });
        }

        private static void AddJwtAuthentication(this SwaggerGenOptions options)
        {
            // Bearer token authentication
            OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
            {
                Name = "Bearer",
                BearerFormat = "JWT",
                Scheme = "bearer",
                Description = "Specify the authorization token.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
            };
            options.AddSecurityDefinition("jwt_auth", securityDefinition);

            // Make sure swagger UI requires a Bearer token specified
            var securityScheme = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Id = "jwt_auth",
                    Type = ReferenceType.SecurityScheme
                }
            };
            var securityRequirements = new OpenApiSecurityRequirement()
            {
                {securityScheme, new string[] { }},
            };
            options.AddSecurityRequirement(securityRequirements);
        }

        private static void AddComments(this SwaggerGenOptions options)
        {
            var controllersAssembly = typeof(StockController).Assembly;
            var xmlFile = $"{controllersAssembly.GetName().Name}.xml";
            var xmlPath = GetFullFilePath(xmlFile);
            options.IncludeXmlComments(xmlPath);
        }

        private static string GetFullFilePath(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, fileName);
        }

        internal static IApplicationBuilder UseSwaggerMiddlewares(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                // todo: вынести в константы
                c.SwaggerEndpoint("v1/swagger.json", "Hamster API v1") // путь относительно web-приложения
            );
            return app;
        }
    }
}