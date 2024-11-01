namespace Zz.Pagination;

using System.Collections;

public interface IPaginationValues<out T> : IEnumerable<T>
{
    public IEnumerable<T> Values { get; }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();
}
