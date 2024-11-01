namespace Zz.App.Core;

using System;
using Microsoft.Extensions.DependencyInjection;

public abstract partial class ProcessHandler
{
    private TaskManager? _TaskManager;
    public TaskManager TaskManager => _TaskManager ??= GetRequiredService<TaskManager>();

    public async Task<TaskManager.ScheduledTask> RunScheduleTask(TaskManager.ScheduledTask task)
    {
        return await TaskManager.RunScheduledTask(task);
    }

    public TaskManager.ScheduledTask ScheduleScopedTask(
        Func<IServiceScope, Task> task,
        string? taskId,
        TaskManager.TaskPriority taskPriority = TaskManager.TaskPriority.Regular
    )
    {
        return TaskManager.ScheduleScopedTask(task, this.ProcessorInfo.Id, taskId, taskPriority);
    }

    public TaskManager.ScheduledTask ScheduleTask(
        Func<Task> task,
        string? taskId = null,
        TaskManager.TaskPriority taskPriority = TaskManager.TaskPriority.Regular
    )
    {
        return TaskManager.CreateScheduledTask(task, this.ProcessorInfo.Id, taskId, taskPriority);
    }

    public TaskManager.ScheduledTask<TRes> ScheduleTask<TRes>(
        Func<Task<TRes>> task,
        string? taskId = null,
        TaskManager.TaskPriority taskPriority = TaskManager.TaskPriority.RealTime
    )
    {
        return TaskManager.CreateScheduledTask(task, this.ProcessorInfo.Id, taskId, taskPriority);
    }
}
