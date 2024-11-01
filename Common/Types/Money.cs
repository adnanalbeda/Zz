using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Zz;

[ComplexType]
public sealed class Money : IEquatable<Money>, IComparable<Money>, ICommonType
{
    public Money(decimal amount, string? currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            this.Currency = string.Empty;
        else
        {
            AppArgumentException.ThrowIfInvalidCurrencyFormat(currency);
            this.Currency = currency.ToUpperInvariant();
        }

        if (default != amount && string.IsNullOrEmpty(this.Currency))
            throw new ArgumentException("Cannot define amount without currency.", nameof(amount));

        Amount = amount;
    }

    [Precision(18, 6)]
    public decimal Amount { get; }

    /// <summary>
    /// ISO 3A Currency Code
    /// </summary>
    public string Currency { get; }

    private static Money _empty = new(default, default);
    public static Money Empty => _empty;

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Amount, this.Currency);
    }

    public override bool Equals(object? obj)
    {
        return obj is Money money && Equals(money);
    }

    public bool Equals(Money? other)
    {
        return other is not null
            && this.Amount == other.Amount
            && this.Currency.Equals(other.Currency, StringComparison.InvariantCultureIgnoreCase);
    }

    public int CompareTo(Money? other)
    {
        if (other is null)
            return 1;

        if (this.Currency.Equals(other.Currency, StringComparison.InvariantCultureIgnoreCase))
            return this.Amount.CompareTo(other.Amount);

        return this.Currency.CompareTo(other.Currency);
    }

    public static bool operator ==(Money? left, Money? right)
    {
        return left is null ? right is null : left.Equals(right);
    }

    public static bool operator !=(Money? left, Money? right)
    {
        return !(left == right);
    }

    public static bool operator <(Money? left, Money? right)
    {
        return ReferenceEquals(left, null)
            ? !ReferenceEquals(right, null)
            : left.CompareTo(right) < 0;
    }

    public static bool operator <=(Money? left, Money? right)
    {
        return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
    }

    public static bool operator >(Money? left, Money? right)
    {
        return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
    }

    public static bool operator >=(Money? left, Money? right)
    {
        return ReferenceEquals(left, null)
            ? ReferenceEquals(right, null)
            : left.CompareTo(right) >= 0;
    }
}
