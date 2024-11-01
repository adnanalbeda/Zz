using System.ComponentModel.DataAnnotations;

namespace Zz.Model;

public interface ICurrency
{
    public string Code { get; }
    public string? Symbol { get; }
}

// Use for all required field (and possibly for a list all as options or quick search)
public class CurrencyBase : ICurrency
{
    public CurrencyBase(string code, string? symbol = null)
    {
        AppArgumentException.ThrowIfInvalidCurrencyFormat(code);
        Code = code.ToUpperInvariant();
        Symbol = symbol;
    }

    public string Code { get; set; }

    [MinLength(1)]
    [MaxLength(3)]
    public string? Symbol { get; set; }
}

// Use for (List - Create - Update)
public class CurrencyDto(string code, string? symbol = null) : CurrencyBase(code, symbol)
{
    public long? Id { get; init; }

    public string? FullName { get; init; }

    public bool IsSupported { get; set; } = true;
}

// Use for shape of all necessary data (and List [by Privileged Accounts])
public class CurrencyTable(string code, string? symbol = null)
    : CurrencyDto(code, symbol),
        ITrackChangeDoer
{
    public new long Id
    {
        get => base.Id ?? default;
        init => base.Id = default == value ? null : value;
    }

    #region ITrack Members

    public DateTime Track_CreatedAt_ { get; private init; } = UtcNow;
    public DateTime Track_UpdatedAt_ { get; set; } = UtcNow;

    public Zz.Identity Track_DoneBy_ { get; set; } = Zz.Identity.Unknown;

    #endregion
}

// Use for (Details)
public class Currency(string code, string? symbol = null)
    : CurrencyTable(code, symbol),
        ITrackChangeDoer<Currency>
{
    // DX: Relation
    public List<MoneyFXRate> FXRatesHistoryAsSource { get; set; } = new();

    // DX: Relation
    public List<MoneyFXRate> FXRatesHistoryAsTarget { get; set; } = new();
}

public static partial class FXDataSeed
{
    public static IEnumerable<Currency> Currencies() =>
        [
            new("USD")
            {
                FullName = "US Dollar",
                Symbol = "$",
                IsSupported = true,
            },
            new("EUR")
            {
                FullName = "Euro",
                Symbol = "€",
                IsSupported = true,
            },
            new("GBP")
            {
                FullName = "UK Pound",
                Symbol = "£",
                IsSupported = true,
            },
        ];
}
