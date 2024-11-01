namespace Zz.App.Core;

using System;
using Microsoft.Extensions.DependencyInjection;

public partial class TaskManager
{
    public ScheduledTask ScheduleScopedTask(
        Func<IServiceScope, Task> scopedTask,
        string parentProcessId,
        string? taskId = null,
        TaskPriority priority = TaskPriority.Regular
    )
    {
        return CreateScheduledTask(
            async () =>
            {
                using var scope = this.ServiceScopeFactory.CreateAsyncScope();
                await scopedTask(scope);
            },
            parentProcessId,
            taskId,
            priority
        );
    }
}
