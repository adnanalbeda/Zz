using System.Collections;
using System.Collections.Immutable;

namespace Zz.App.Core;

public partial class TaskManager
{
    public static TimeSpan TimeToPurge { get; } = TimeSpan.FromMinutes(5);

    public sealed class TaskScheduler : IList<ScheduledTask>
    {
        private readonly List<ScheduledTask> _tasks = new(64);

        /// <summary>
        /// </summary>
        /// <param name="olderThan">Uses is the default if null.</param>
        /// <returns></returns>
        public int PurgeSuccess(TimeSpan? olderThan)
        {
            var date = DateTime.UtcNow.Add(
                olderThan ?? (TimeToPurge > TimeSpan.Zero ? -1 * TimeToPurge : TimeToPurge)
            );

            var p = _tasks
                .Where(x => x.Status is TaskStatus.Success && x.CreatedAt <= date)
                .ToImmutableArray();

            foreach (var t in p)
            {
                _tasks.Remove(t);
            }

            return p.Length;
        }

        /// <summary>
        /// </summary>
        /// <param name="olderThan">Uses is the default if null.</param>
        /// <returns></returns>
        public int PurgeError(TimeSpan? olderThan)
        {
            var date = DateTime.UtcNow.Add(
                olderThan ?? (TimeToPurge > TimeSpan.Zero ? -1 * TimeToPurge : TimeToPurge)
            );

            var p = _tasks
                .Where(x => x.Status is TaskStatus.Error && x.CreatedAt <= date)
                .ToImmutableArray();

            foreach (var t in p)
            {
                _tasks.Remove(t);
            }

            return p.Length;
        }

        public ScheduledTask this[int index]
        {
            get => ((IList<ScheduledTask>)_tasks)[index];
            set => ((IList<ScheduledTask>)_tasks)[index] = value;
        }

        public int Count => ((ICollection<ScheduledTask>)_tasks).Count;

        public bool IsReadOnly => ((ICollection<ScheduledTask>)_tasks).IsReadOnly;

        public void Add(ScheduledTask item)
        {
            if (_tasks.Contains(item))
                return;
            ((ICollection<ScheduledTask>)_tasks).Add(item);
            _tasks.Sort();
        }

        public void Clear()
        {
            throw new Exception("What the hell are you trying to do?");
            // ((ICollection<ScheduledTask>)_tasks).Clear();
        }

        public bool Contains(ScheduledTask item)
        {
            return ((ICollection<ScheduledTask>)_tasks).Contains(item);
        }

        public void CopyTo(ScheduledTask[] array, int arrayIndex)
        {
            ((ICollection<ScheduledTask>)_tasks).CopyTo(array, arrayIndex);
        }

        public IEnumerator<ScheduledTask> GetEnumerator()
        {
            return ((IEnumerable<ScheduledTask>)_tasks).GetEnumerator();
        }

        public int IndexOf(ScheduledTask item)
        {
            return ((IList<ScheduledTask>)_tasks).IndexOf(item);
        }

        public void Insert(int index, ScheduledTask item)
        {
            ((IList<ScheduledTask>)_tasks).Insert(index, item);
        }

        public bool Remove(ScheduledTask item)
        {
            var res = ((ICollection<ScheduledTask>)_tasks).Remove(item);
            _tasks.Sort();
            return res;
        }

        public void RemoveAt(int index)
        {
            ((IList<ScheduledTask>)_tasks).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_tasks).GetEnumerator();
        }
    }
}
