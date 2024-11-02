using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Zz.App.Core;
using Zz.DataBase;
using Zz.Model.Identity;

namespace Zz;

public static partial class AppDependencyInjectionExtensions
{
    public static IServiceCollection RegisterZzIdentityService<TI>(this IServiceCollection sc)
        where TI : class, IUserAccessor
    {
        sc.AddScoped<IUserAccessor, TI>();
        return sc;
    }
}
