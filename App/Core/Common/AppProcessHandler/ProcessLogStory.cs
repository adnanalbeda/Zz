using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Zz.App.Core;

public abstract partial class AppProcessHandler<T>
{
    private ProcessLogStory<T>? _AppProcessLogger;
    protected ProcessLogStory<T> AppProcessLogger =>
        _AppProcessLogger ??= GetRequiredService<ProcessLogStory<T>>();
}
