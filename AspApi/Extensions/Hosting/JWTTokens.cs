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

        var identityConfigs = new ZzAppConfigs(builder.Configuration).IdentityConfigs!;

        string[] wsEventsPaths = ["/signalr", "/notification"];

        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityConfigs.JwtKey!));

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuers = [identityConfigs.Issuer ?? "Zz"],
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero,
                };

                // Support encryption?
                // if (!string.IsNullOrWhiteSpace(identityConfigs.JwtEncryptionCert?.PathToPrivate))
                // {
                //     opt.TokenValidationParameters.TokenDecryptionKey = new X509SecurityKey(
                //         new X509Certificate2(identityConfigs.JwtEncryptionCert.PathToPrivate)
                //     );
                // }
                // else if (!string.IsNullOrWhiteSpace(identityConfigs.JwtEncryptionKey))
                // {
                //     var encryptionKey = new SymmetricSecurityKey(
                //         Encoding.UTF8.GetBytes(identityConfigs.JwtEncryptionKey!)
                //     );
                //     opt.TokenValidationParameters.TokenDecryptionKey = encryptionKey;
                // }

                opt.Events = new JwtBearerEvents
                {
                    // Add support for WebSocket protocol or SignalR
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        // var sessionKey = context.Request.Query["session_key"];
                        var path = context.HttpContext.Request.Path;
                        if (
                            !string.IsNullOrEmpty(accessToken)
                            && wsEventsPaths.Any(x =>
                                path.StartsWithSegments(
                                    x,
                                    StringComparison.InvariantCultureIgnoreCase
                                )
                            )
                        )
                        {
                            context.Token = accessToken;
                            // context.Request.Headers["SessionKey"] = sessionKey;
                            return Task.CompletedTask;
                        }

                        // if (string.IsNullOrWhiteSpace(context.Token))
                        // {
                        //     context.Token = context.HttpContext.Request.Cookies["Authentication"];
                        // }
                        return Task.CompletedTask;
                    },
                };
            });
        return builder;
    }
}
