using System.Runtime.CompilerServices;

namespace Zz;

public interface IGuard<T, TValue> : IEquatable<TValue>, IComparable<TValue>
    where T : IGuard<T, TValue>, new()
{
    public TValue? Value { get; }

    public void SetValue(
        TValue? argument,
        [CallerArgumentExpression("argument")] string paramName = ""
    );
}
