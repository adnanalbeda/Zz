using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Zz.App.Core;

public abstract partial class AppProcessHandler<T>
{
    private IMediator? _Mediator;
    protected IMediator Mediator => _Mediator ??= GetRequiredService<IMediator>();

    public void PublishNotification<TNotification>(
        TNotification notification,
        TaskManager.TaskPriority priority = TaskManager.TaskPriority.Regular
    )
        where TNotification : IAppNotification
    {
        this.ScheduleScopedTask(
            async (scope) =>
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(notification);
            },
            notification.NotificationTaskId,
            priority
        );
    }
}
