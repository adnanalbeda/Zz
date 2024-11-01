namespace Zz.App.Core;

using System;
using Microsoft.Extensions.Logging;

public partial class TaskManager
{
    public enum TaskPriority
    {
        Slack,
        Regular,
        ASAP,
        RealTime,
    }

    public enum TaskStatus
    {
        Waiting,
        Running,
        Success,
        Error,
    }

    private static readonly TaskScheduler _Tasks = new();
    public static TaskScheduler Tasks => _Tasks;

    public ScheduledTask<TRes> CreateScheduledTask<TRes>(
        Func<Task<TRes>> task,
        string parentProcessId,
        string? taskId = null,
        TaskPriority priority = TaskPriority.Regular
    )
    {
        var st = new ScheduledTask<TRes>(task, parentProcessId, taskId, priority);

        _Tasks.Add(st);

        this.Logger.LogDebug("Scheduled Result Task: {PID} - {TID}", st.ParentProcessId, st.TaskId);

        return st;
    }

    public ScheduledTask CreateScheduledTask(
        Func<Task> task,
        string parentProcessId,
        string? taskId = null,
        TaskPriority priority = TaskPriority.Regular
    )
    {
        var st = new ScheduledTask(task, parentProcessId, taskId, priority);

        _Tasks.Add(st);

        this.Logger.LogDebug("Scheduled Task: {PID} - {TID}", st.ParentProcessId, st.TaskId);

        return st;
    }

    public async Task<ScheduledTask> RunScheduledTask(ScheduledTask st)
    {
        if (st.Status is TaskStatus.Success or TaskStatus.Error)
            return st;

        if (st.Status is not TaskStatus.Running)
        {
            this.Logger.LogDebug(
                "Running Scheduled Task: {PID} - {TID}",
                st.ParentProcessId,
                st.TaskId
            );

            st.UpdateStatus(TaskStatus.Running);
        }
        try
        {
            await st.Task();
            st.UpdateStatus(TaskStatus.Success);
            this.Logger.LogDebug(
                "Scheduled Task (SUCCESS): {PID} - {TID}",
                st.ParentProcessId,
                st.TaskId
            );
        }
        catch (Exception ex)
        {
            st.UpdateStatus(TaskStatus.Error);
            this.Logger.LogError(
                ex,
                "Scheduled Task (ERROR): {PID} - {TID} - {EX_MSG} - {EX_INNER_MSG}",
                st.ParentProcessId,
                st.TaskId,
                ex.Message,
                ex.InnerException?.Message
            );
        }
        return st;
    }
}
