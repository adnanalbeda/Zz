using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Zz;
using Zz.DataBase;
using Zz.Model.Identity;
using Zz.Services;

namespace System;

public static partial class ZzDependencyInjectionExtensions
{
    public static IHostApplicationBuilder AddZzAppIdentity(this IHostApplicationBuilder builder)
    {
        // Just cause it says view doesn't necessarily mean an html.
        // It's just name of a pattern.
        // The view in this case is api json result.
        Console.WriteLine("Configure App DB Service...");

        builder
            .Services.RegisterZzIdentityService<UserAccessor>()
            .AddIdentityCore<User>(options =>
            {
                // Register
                options.User = new() { RequireUniqueEmail = false };
                options.Password = new()
                {
                    RequiredLength = 8,
                    RequiredUniqueChars = 4,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireNonAlphanumeric = true,
                };
                // Login
                options.SignIn = new() { RequireConfirmedEmail = false };
                options.Lockout = new()
                {
                    AllowedForNewUsers = true,
                    MaxFailedAccessAttempts = 5,
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10),
                };

                options.Tokens.AuthenticatorIssuer = "Zz";
            })
            .AddEntityFrameworkStores<DataContext>();

        return builder;
    }
}
