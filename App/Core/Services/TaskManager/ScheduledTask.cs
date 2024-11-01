namespace Zz.App.Core;

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Query;

public partial class TaskManager
{
    public sealed class ScheduledTask<TRes> : ScheduledTask
    {
        internal ScheduledTask(
            Func<Task<TRes>> task,
            string parentProcessId,
            string? taskId = null,
            TaskPriority priority = TaskPriority.RealTime // scheduled tasks shouldn't have a result, so this one is most likely a real time one.
        )
            : base(task, parentProcessId, taskId, priority)
        {
            ResultTask = task;
        }

        public Func<Task<TRes>> ResultTask { get; }
        public override Func<Task> Task => ResultTask;
    }

    public class ScheduledTask
        : IEquatable<ScheduledTask>,
            IEqualityComparer<ScheduledTask>,
            IComparable<ScheduledTask>
    {
        public virtual Func<Task> Task { get; }
        public string ParentProcessId { get; }
        public TaskPriority Priority { get; }

        public string TaskId { get; }

        private TaskStatus _Status = TaskStatus.Waiting;
        public TaskStatus Status => _Status;

        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        internal ScheduledTask(
            Func<Task> task,
            string parentProcessId,
            string? taskId = null,
            TaskPriority priority = TaskPriority.Regular
        )
        {
            Task = task;
            ParentProcessId = parentProcessId;
            Priority = priority;

            TaskId = taskId ?? CreatedAt.ToShortId(DateTimeHelpers.ShortIdEndWith.Millisecond);
        }

        internal void UpdateStatus(TaskStatus status)
        {
            // new value can't be lower than current status
            if (status is TaskStatus.Waiting)
                return;

            // and new value can't be applied to done tasks.
            if (this.Status is TaskStatus.Success or TaskStatus.Error)
                return;

            // now status is either waiting or running, so set new status anyway.
            this._Status = status;
        }

        public bool Equals(ScheduledTask? other)
        {
            return other is not null
                && this.ParentProcessId.Equals(other.ParentProcessId)
                && this.TaskId.Equals(other.TaskId);
        }

        public override bool Equals(object? obj)
        {
            return obj is ScheduledTask st && Equals(st);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.ParentProcessId, this.TaskId);
        }

        public int CompareTo(ScheduledTask? other)
        {
            if (other is null) // Current is more important than null
                return -1;
            if (this.Status is TaskStatus.Waiting)
                return other.Status is TaskStatus.Waiting
                    ? this.Priority.CompareTo(other.Priority)
                    : -1; // Current is more important for new run than already running or finished one.
            if (this.Status is TaskStatus.Success or TaskStatus.Error)
                return other.Status is TaskStatus.Waiting or TaskStatus.Running ? 1 : 0; // Others are more important to work if this one is done.
            return this.CreatedAt.CompareTo(other.CreatedAt); // Anything else is the same, order by create time (oldest first).
        }

        public bool Equals(ScheduledTask? x, ScheduledTask? y)
        {
            return x is null
                ? y is null
                : y is not null
                    && x.ParentProcessId.Equals(y.ParentProcessId)
                    && x.TaskId.Equals(y.TaskId);
        }

        public int GetHashCode([DisallowNull] ScheduledTask obj)
        {
            return HashCode.Combine(obj.ParentProcessId, obj.TaskId);
        }

        public static bool operator ==(ScheduledTask? left, ScheduledTask? right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ScheduledTask? left, ScheduledTask? right)
        {
            return !(left == right);
        }

        public static bool operator <(ScheduledTask? left, ScheduledTask? right)
        {
            return ReferenceEquals(left, null)
                ? !ReferenceEquals(right, null)
                : left.CompareTo(right) < 0;
        }

        public static bool operator <=(ScheduledTask left, ScheduledTask right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(ScheduledTask left, ScheduledTask right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(ScheduledTask left, ScheduledTask right)
        {
            return ReferenceEquals(left, null)
                ? ReferenceEquals(right, null)
                : left.CompareTo(right) >= 0;
        }
    }
}
