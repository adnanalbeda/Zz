namespace Zz.App.Core;

using System;
using Microsoft.Extensions.DependencyInjection;

public abstract partial class ProcessHandler(IServiceProvider sp)
{
    private IServiceProvider serviceProvider = sp;
    protected IServiceProvider ServiceProvider => serviceProvider;

    protected TService? GetService<TService>() => ServiceProvider.GetService<TService>();

    protected TService GetRequiredService<TService>()
        where TService : notnull => ServiceProvider.GetRequiredService<TService>();

    protected TService? GetKeyedService<TService>(object? serviceKey) =>
        ServiceProvider.GetKeyedService<TService>(serviceKey);

    protected TService GetRequiredKeyedService<TService>(object? serviceKey)
        where TService : notnull => ServiceProvider.GetRequiredKeyedService<TService>(serviceKey);
}
