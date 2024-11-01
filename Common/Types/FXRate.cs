using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Zz;

[ComplexType]
public class FXRate
    : IEquatable<FXRate>,
        IEqualityComparer<FXRate>,
        IComparable<FXRate>,
        ICommonType
{
    public FXRate(string source, string target, decimal rates)
    {
        AppArgumentException.ThrowIfInvalidCurrencyFormat(source);
        AppArgumentException.ThrowIfInvalidCurrencyFormat(target);

        this.Source = source.ToUpperInvariant();
        this.Target = target.ToUpperInvariant();

        if (rates <= 0)
        {
            if (!Source.Equals(Target, StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("FXRate can't be negative or zero.", nameof(rates));

            this.Rates = 1;
            return;
        }

        this.Rates = rates;
    }

    /// <summary>
    /// ISO 3A Currency Code
    /// </summary>
    public string Source { get; private set; }

    /// <summary>
    /// ISO 3A Currency Code
    /// </summary>
    public string Target { get; private set; }

    [Precision(18, 8)]
    public decimal Rates { get; private set; }

    public int GetHashCode([DisallowNull] FXRate obj)
    {
        return HashCode.Combine(obj.Source, obj.Target, obj.Rates);
    }

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    public bool Equals(FXRate? other)
    {
        return other is not null
            && this.Rates == other.Rates
            && this.Target.Equals(other.Target, StringComparison.InvariantCultureIgnoreCase)
            && this.Source.Equals(other.Source, StringComparison.InvariantCultureIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as FXRate);
    }

    public int CompareTo(FXRate? other)
    {
        if (other is null)
            return 1;

        if (!this.Source.Equals(other.Source, StringComparison.InvariantCultureIgnoreCase))
            return this.Source.CompareTo(other.Source);

        if (!this.Target.Equals(other.Target, StringComparison.InvariantCultureIgnoreCase))
            return this.Target.CompareTo(other.Target);

        return this.Rates.CompareTo(other.Rates);
    }

    public bool Equals(FXRate? x, FXRate? y)
    {
        return x?.Equals(y) ?? y is null;
    }

    public static bool operator ==(FXRate? left, FXRate? right)
    {
        return left is null ? right is null : left.Equals(right);
    }

    public static bool operator !=(FXRate? left, FXRate? right)
    {
        return !(left == right);
    }

    public static bool operator <(FXRate? left, FXRate? right)
    {
        return ReferenceEquals(left, null)
            ? !ReferenceEquals(right, null)
            : left.CompareTo(right) < 0;
    }

    public static bool operator <=(FXRate? left, FXRate? right)
    {
        return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
    }

    public static bool operator >(FXRate? left, FXRate? right)
    {
        return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
    }

    public static bool operator >=(FXRate? left, FXRate? right)
    {
        return ReferenceEquals(left, null)
            ? ReferenceEquals(right, null)
            : left.CompareTo(right) >= 0;
    }
}
