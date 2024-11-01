using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Core;
using Zz.App;
using Zz.App.Core;

namespace Zz;

public static partial class AppDependencyInjectionExtensions
{
    public static IServiceCollection RegisterMediator(
        this IServiceCollection sc,
        params Assembly[] assemblies
    )
    {
        sc.AddMediatR(configs =>
        {
            configs
                .AddOpenBehavior(typeof(RequestEntryExitPipeline<,>))
                .AddOpenBehavior(typeof(CancellationHandlerPipeline<,>))
                .AddOpenBehavior(typeof(ExceptionHandlerPipeline<,>))
                .AddOpenBehavior(typeof(ValidationPipeline<,>))
                .RegisterServicesFromAssemblies(
                    assemblies
                        .Prepend(typeof(IAppProcess).Assembly)
                        .Append(Assembly.GetExecutingAssembly())
                        .ToArray()
                );
        });
        return sc;
    }

    public static IServiceCollection RegisterMediator<TAM>(this IServiceCollection sc) =>
        sc.RegisterMediator(typeof(TAM).Assembly);

    public static IServiceCollection RegisterMediator<TAM1, TAM2>(this IServiceCollection sc) =>
        sc.RegisterMediator(typeof(TAM1).Assembly, typeof(TAM2).Assembly);

    public static IServiceCollection RegisterMediator<TAM1, TAM2, TAM3>(
        this IServiceCollection sc
    ) => sc.RegisterMediator(typeof(TAM1).Assembly, typeof(TAM2).Assembly, typeof(TAM3).Assembly);
}
