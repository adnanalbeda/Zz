namespace Zz.App.Core;

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public partial class TaskManager(IServiceScopeFactory serviceScopeFactory)
{
    public IServiceScopeFactory ServiceScopeFactory { get; } = serviceScopeFactory;
    public IServiceProvider ServiceProvider { get; } =
        serviceScopeFactory.CreateScope().ServiceProvider;

    private ILogger? logger;
    protected ILogger Logger => logger ??= GetRequiredService<ILogger<TaskManager>>();

    protected TService? GetService<TService>() => ServiceProvider.GetService<TService>();

    protected TService GetRequiredService<TService>()
        where TService : notnull => ServiceProvider.GetRequiredService<TService>();

    protected TService? GetKeyedService<TService>(object? serviceKey) =>
        ServiceProvider.GetKeyedService<TService>(serviceKey);

    protected TService GetRequiredKeyedService<TService>(object? serviceKey)
        where TService : notnull => ServiceProvider.GetRequiredKeyedService<TService>(serviceKey);
}
