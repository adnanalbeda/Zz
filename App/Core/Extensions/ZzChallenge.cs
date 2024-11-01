using Microsoft.Extensions.DependencyInjection;
using Zz.App.Core;
using Zz.Model.Identity;

namespace Zz;

public static partial class AppDependencyInjectionExtensions
{
    public static IServiceCollection RegisterZzChallengeService<TI>(this IServiceCollection sc)
        where TI : class, IChallenge
    {
        sc.AddTransient<IChallenge, TI>();
        return sc;
    }
}
