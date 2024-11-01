using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Core;
using Zz.App;
using Zz.App.Core;
using Zz.AutoMapper;

namespace Zz;

public static partial class AppDependencyInjectionExtensions
{
    public static IServiceCollection RegisterAutoMapper(
        this IServiceCollection sc,
        params Assembly[] assemblies
    )
    {
        sc.AddAutoMapper(
            assemblies
                .Prepend(typeof(MappingProfileBuilder).Assembly)
                .Append(Assembly.GetExecutingAssembly())
                .ToArray()
        );
        return sc;
    }

    public static IServiceCollection RegisterAutoMapper<TAM>(this IServiceCollection sc) =>
        sc.RegisterAutoMapper(typeof(TAM).Assembly);

    public static IServiceCollection RegisterAutoMapper<TAM1, TAM2>(this IServiceCollection sc) =>
        sc.RegisterAutoMapper(typeof(TAM1).Assembly, typeof(TAM2).Assembly);

    public static IServiceCollection RegisterAutoMapper<TAM1, TAM2, TAM3>(
        this IServiceCollection sc
    ) => sc.RegisterAutoMapper(typeof(TAM1).Assembly, typeof(TAM2).Assembly, typeof(TAM3).Assembly);
}
