using Microsoft.Extensions.DependencyInjection;
using Zz.App.Core;
using Zz.Model.Identity;

namespace Zz;

public static partial class AppDependencyInjectionExtensions
{
    public static IServiceCollection RegisterZzEmailSenderService<TI>(this IServiceCollection sc)
        where TI : class, IEmailSender
    {
        sc.AddTransient<IEmailSender, TI>();
        return sc;
    }
}
