using Microsoft.Extensions.DependencyInjection;
using Zz.App.Core;
using Zz.Model.Identity;

namespace Zz;

public static partial class AppDependencyInjectionExtensions
{
    public static IServiceCollection RegisterZzMoneyFXService<TI>(this IServiceCollection sc)
        where TI : class, IMoneyFX
    {
        sc.AddTransient<IMoneyFX, TI>();
        return sc;
    }
}
