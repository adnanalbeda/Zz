using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Zz;

public static class LoggerExtensions
{
    public static ILogger<T> GetLogger<T>(this IServiceProvider serviceProvider) =>
        serviceProvider.GetRequiredService<ILogger<T>>();
}
