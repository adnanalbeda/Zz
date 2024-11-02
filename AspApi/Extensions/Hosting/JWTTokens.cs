using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Zz;

namespace System;

public static partial class ZzDependencyInjectionExtensions
{
    public static IHostApplicationBuilder ConfigureZzJwtTokens(this IHostApplicationBuilder builder)
    {
        Console.WriteLine("Configure JWT Services...");

        var identityConfigs = new ZzAppConfigsResolver(builder.Configuration).JwtIdentityConfigs!;

        var jwtSigningKey = identityConfigs.SigningKey!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey));

        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                (opt) =>
                {
                    string[] wsEventsPaths = ["/signalr", "/notification"];

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateLifetime = true,

                        IssuerSigningKey = key,
                        ValidateIssuerSigningKey = true,

                        ValidIssuers = identityConfigs.ValidIssuer,
                        ValidateIssuer = true,

                        ValidAudiences = identityConfigs.ValidAudiences,
                        ValidateAudience = true,
                    };

                    opt.Events = new JwtBearerEvents
                    {
                        // Add support for WebSocket protocol or SignalR
                        OnMessageReceived = context =>
                        {
                            var accessToken =
                                context
                                    .HttpContext.Request.Cookies.FirstOrDefault(x =>
                                        x.Key == "access_token"
                                    )
                                    .Value ?? context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (
                                wsEventsPaths.Any(x =>
                                    path.StartsWithSegments(
                                        x,
                                        StringComparison.InvariantCultureIgnoreCase
                                    )
                                )
                            )
                            {
                                context.Token = accessToken;

                                var sessionKey = context.Request.Query["session_key"];
                                context.HttpContext.Request.Headers.Append(
                                    "SessionKey",
                                    sessionKey
                                );

                                return Task.CompletedTask;
                            }

                            context.Token = accessToken;

                            return Task.CompletedTask;
                        },
                    };
                }
            );
        builder.Services.AddAuthorization();

        return builder;
    }
}
