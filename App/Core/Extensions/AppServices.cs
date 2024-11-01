using Microsoft.Extensions.DependencyInjection;
using Zz.App.Core;
using Zz.Model.Identity;

namespace Zz;

public static partial class AppDependencyInjectionExtensions
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection sc)
    {
        sc.AddHttpClient();

        sc.AddSingleton(sc => new TaskManager(sc.GetRequiredService<IServiceScopeFactory>()));
        sc.AddScoped(sc => new ProcessLogStory(
            sc.GetRequiredService<IProcessInfo>(),
            sc.GetService<IRequestInfo>()
        ));
        sc.AddScoped(typeof(ProcessLogStory<>));

        return sc;
    }
}
