// I proudly stole this from MediatR.
// If you don't like something, complain to them for making MediatR.Unit

using System.Runtime.InteropServices;

namespace Zz;

// Summary:
//     Represents a void type, since System.Void is not a valid return type in C#.
[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct Null : IEquatable<Null>, IComparable<Null>, IComparable, ICommonType
{
    private static readonly Null _value = default;

    //
    // Summary:
    //     Default and only value of the _Void type.
    public static ref readonly Null Value => ref _value;

    //
    // Summary:
    //     Task from a Null type.
    public static Task<Null> ValueAsync { get; } = Task.FromResult(Value);

    //
    // Summary:
    //     Compares the current object with another object of the same type.
    //
    // Parameters:
    //   other:
    //     An object to compare with this object.
    //
    // Returns:
    //     A value that indicates the relative order of the objects being compared. The
    //     return value has the following meanings: - Less than zero: This object is less
    //     than the other parameter. - Zero: This object is equal to other. - Greater than
    //     zero: This object is greater than other.
    public int CompareTo(Null other)
    {
        return 0;
    }

    //
    // Summary:
    //     Compares the current instance with another object of the same type and returns
    //     an integer that indicates whether the current instance precedes, follows, or
    //     occurs in the same position in the sort order as the other object.
    //
    // Parameters:
    //   obj:
    //     An object to compare with this instance.
    //
    // Returns:
    //     A value that indicates the relative order of the objects being compared. The
    //     return value has these meanings: - Less than zero: This instance precedes obj
    //     in the sort order. - Zero: This instance occurs in the same position in the sort
    //     order as obj. - Greater than zero: This instance follows obj in the sort order.
    int IComparable.CompareTo(object? obj)
    {
        return 0;
    }

    //
    // Summary:
    //     Returns a hash code for this instance.
    //
    // Returns:
    //     A hash code for this instance, suitable for use in hashing algorithms and data
    //     structures like a hash table.
    public override int GetHashCode()
    {
        return 0;
    }

    //
    // Summary:
    //     Determines whether the current object is equal to another object of the same
    //     type.
    //
    // Parameters:
    //   other:
    //     An object to compare with this object.
    //
    // Returns:
    //     true if the current object is equal to the other parameter; otherwise, false.
    public bool Equals(Null other)
    {
        return true;
    }

    //
    // Summary:
    //     Determines whether the specified System.Object is equal to this instance.
    //
    // Parameters:
    //   obj:
    //     The object to compare with the current instance.
    //
    // Returns:
    //     true if the specified System.Object is equal to this instance; otherwise, false.
    public override bool Equals(object? obj)
    {
        return obj is Null;
    }

    //
    // Summary:
    //     Determines whether the first object is equal to the second object.
    //
    // Parameters:
    //   first:
    //     The first object.
    //
    //   second:
    //     The second object.
    public static bool operator ==(Null first, Null second)
    {
        return true;
    }

    //
    // Summary:
    //     Determines whether the first object is not equal to the second object.
    //
    // Parameters:
    //   first:
    //     The first object.
    //
    //   second:
    //     The second object.
    public static bool operator !=(Null first, Null second)
    {
        return false;
    }

    public static bool operator <(Null left, Null right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Null left, Null right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Null left, Null right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Null left, Null right)
    {
        return left.CompareTo(right) >= 0;
    }

    //
    // Summary:
    //     Returns a System.String that represents this instance.
    //
    // Returns:
    //     A System.String that represents this instance.
    public override string ToString()
    {
        return "()";
    }
}
