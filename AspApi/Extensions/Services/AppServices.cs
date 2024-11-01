using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Zz.App.Core;
using Zz.App.Identity;
using Zz.DataBase;
using Zz.Model.Identity;
using Zz.Services;
using Zz.Services.Identity;

namespace Zz;

public static partial class ZzDependencyInjectionExtensions
{
    public static IServiceCollection AddZzAppServices(this IServiceCollection sc)
    {
        // Just cause it says view doesn't necessarily mean an html.
        // It's just name of a pattern.
        // The view in this case is api json result.
        Console.WriteLine("Configure App Service...");

        sc.RegisterAppServices()
            .RegisterAutoMapper()
            .RegisterFluentValidation()
            .RegisterMediator()
            .RegisterZzMoneyFXService<MoneyFX>()
            .RegisterZzEmailSenderService<EmailSender>()
            .RegisterZzChallengeService<Challenge>()
            .AddScoped<IProcessInfo>(_ => new ProcessInfo())
            .AddScoped<IRequestInfo, RequestInfo>()
            .AddScoped<ITokenService, TokenService>();

        return sc;
    }
}
