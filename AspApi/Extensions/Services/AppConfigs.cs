using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Zz.App.Core;
using Zz.App.Identity;
using Zz.Configs;
using Zz.DataBase;
using Zz.Model.Identity;
using Zz.Services;
using Zz.Services.Identity;

namespace Zz;

public static partial class ZzDependencyInjectionExtensions
{
    public static IServiceCollection AddZzAppConfigs(this IServiceCollection sc)
    {
        // Just cause it says view doesn't necessarily mean an html.
        // It's just name of a pattern.
        // The view in this case is api json result.
        Console.WriteLine("Add Services Configurations...");

        sc.AddSingleton<ZzAppConfigs>()
            .AddSingleton(x => x.GetRequiredService<ZzAppConfigs>().IdentityConfigs!)
            .AddSingleton(x => x.GetRequiredService<ZzAppConfigs>().SmtpEmailConfigs!);

        return sc;
    }
}
