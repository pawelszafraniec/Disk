using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using System;
using System.Collections.Generic;
using System.IO;

namespace Disk.Api.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Cookie,
                    Description = "JWT Authorization header using the Bearer scheme."
                });


                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Disk.Api.xml"), true);
                config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Disk.Common.xml"), true);
            });
        }

        public static void UseSwaggerDocs(this IApplicationBuilder app)
        {
            app.UseSwagger(config =>
            {
                config.RouteTemplate = "/{documentName}/api/rest/swagger.json";
            });
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/v1/api/rest/swagger.json", "My API V1");
            });
        }
    }
}
