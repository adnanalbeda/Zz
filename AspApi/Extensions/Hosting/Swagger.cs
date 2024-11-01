#if DEBUG
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Zz;
using Zz.Api.Swagger;
using Zz.Model;

namespace System;

public static partial class ZzDependencyInjectionExtensions
{
    public static IHostApplicationBuilder ConfigureZzSwagger(this IHostApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment())
            return builder;
        /*
        // If versioning is not working, uncomment this.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        */
        // builder.Services.AddTransient<
        //     IConfigureOptions<SwaggerGenOptions>,
        //     VersioningConfigureSwaggerOptions
        // >();
        builder.Services.AddSwaggerGen(
            (opt) =>
            {
                var bearer = "Bearer";

                opt.OperationFilter<SwaggerDefaultValues>();
                opt.SchemaFilter<SwaggerEnumSchemaFilter>();

                // Authentication
                opt.AddSecurityDefinition(
                    bearer,
                    new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. \r\n\r\n"
                            + "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n"
                            + "Example: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = bearer,
                    }
                );
                opt.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = bearer,
                                },
                                Scheme = "oauth2",
                                Name = bearer,
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        },
                    }
                );

                // // Session Key
                // var sessionKey = "SessionKey";
                // // Authentication
                // opt.AddSecurityDefinition(
                //     sessionKey,
                //     new OpenApiSecurityScheme
                //     {
                //         Description = "Session secret key.",
                //         Name = "SessionKey",
                //         In = ParameterLocation.Header,
                //         Type = SecuritySchemeType.Http,
                //         Scheme = bearer,
                //     }
                // );
                // opt.AddSecurityRequirement(
                //     new OpenApiSecurityRequirement()
                //     {
                //         {
                //             new OpenApiSecurityScheme
                //             {
                //                 Reference = new OpenApiReference
                //                 {
                //                     Type = ReferenceType.SecurityScheme,
                //                     Id = sessionKey,
                //                 },
                //                 Scheme = "oauth2",
                //                 Name = sessionKey,
                //                 In = ParameterLocation.Header,
                //             },
                //             new List<string>()
                //         },
                //     }
                // );

                opt.MapType<Id22>(
                    () =>
                        new OpenApiSchema
                        {
                            Type = "string",
                            Description = "Base64URL 22 characters ID. Compatible with Guid value.",
                            Example = new OpenApiString("01234567890AbCdEfGhI-_"),
                        }
                );

                opt.SupportNonNullableReferenceTypes();

                opt.UseAllOfForInheritance();

                var types = typeof(ICommonType)
                    .Assembly.GetTypes()
                    .Concat(typeof(IModelType).Assembly.GetTypes());
                var minimumTypes = types
                    .Where(x => !x.IsGenericType)
                    .Where(x => !x.IsInterface)
                    .Where(x => !x.IsEnum)
                    .ToArray();

                opt.SelectSubTypesUsing(baseType =>
                {
                    return minimumTypes.Where(type => type.IsSubclassOf(baseType));
                });

                // Using [type.ToString().Replace("+", ".")] is to prevent MediatR base inheritance conflict for swagger.
                opt.CustomSchemaIds(type =>
                    type.IsEnum
                        ? string.Concat(type.ToString().Replace("+", "."), "Enum")
                        : (type.FullName ?? string.Concat(type.Namespace, ".", type.Name)).Replace(
                            "+",
                            "."
                        )
                );
            }
        );
        return builder;
    }
}
#endif
