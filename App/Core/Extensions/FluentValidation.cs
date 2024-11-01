using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Core;
using Zz.App;
using Zz.App.Core;

namespace Zz;

public static partial class AppDependencyInjectionExtensions
{
    public static IServiceCollection RegisterFluentValidation(
        this IServiceCollection sc,
        params Assembly[] assemblies
    )
    {
        sc.AddValidatorsFromAssemblies(
            assemblies
                .Prepend(typeof(IMayValidate).Assembly)
                .Append(Assembly.GetExecutingAssembly())
                .ToArray()
        );
        return sc;
    }

    public static IServiceCollection RegisterFluentValidation<TAM>(this IServiceCollection sc) =>
        sc.RegisterFluentValidation(typeof(TAM).Assembly);

    public static IServiceCollection RegisterFluentValidation<TAM1, TAM2>(
        this IServiceCollection sc
    ) => sc.RegisterFluentValidation(typeof(TAM1).Assembly, typeof(TAM2).Assembly);

    public static IServiceCollection RegisterFluentValidation<TAM1, TAM2, TAM3>(
        this IServiceCollection sc
    ) =>
        sc.RegisterFluentValidation(
            typeof(TAM1).Assembly,
            typeof(TAM2).Assembly,
            typeof(TAM3).Assembly
        );
}
