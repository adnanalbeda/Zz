using System.Diagnostics.CodeAnalysis;

namespace Zz;

public interface IEquals<T> : IEqualityComparer<T>, IEquatable<T>
    where T : IEquals<T>
{
    public new bool Equals(T? other);

    bool IEqualityComparer<T>.Equals(T? x, T? y) => x is null ? y is null : x.Equals(y);
    bool IEquatable<T>.Equals(T? other) => Equals(other);
}
