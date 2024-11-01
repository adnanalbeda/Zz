using System.Diagnostics;

namespace Zz;

public class InvalidMoneyOperationException : InvalidOperationException
{
    public InvalidMoneyOperationException(
        string currency1,
        string currency2,
        string? message = null,
        Exception? innerException = null
    )
        : base(message, innerException)
    {
        Currency1 = currency1;
        Currency2 = currency2;
    }

    public string Currency1 { get; }
    public string Currency2 { get; }
}

public class InvalidMoneyFXConvertOperationException : InvalidMoneyOperationException
{
    public InvalidMoneyFXConvertOperationException(
        Money money,
        FXRate fxRate,
        string? message = null,
        Exception? innerException = null
    )
        : base(
            money.Currency,
            fxRate.Target,
            message
                ?? string.Concat(
                    "Cannot convert: ",
                    $"{money.Amount:000.00}({money.Currency})",
                    Environment.NewLine,
                    " with provided FX Rates: ",
                    Environment.NewLine,
                    $"({fxRate.Source})-->({fxRate.Rates:000.000})==>({fxRate.Target})"
                ),
            new UnreachableException(
                string.Concat(
                    $"(MONEY_CURRENCY):{nameof(money)}.{nameof(money.Currency)}:({money.Currency})",
                    " cannot be converted with: ",
                    Environment.NewLine,
                    $"(FX_SOURCE_CURRENCY):{nameof(fxRate)}.{nameof(fxRate.Source)}:({fxRate.Source})",
                    Environment.NewLine,
                    $"(FX_TARGET_CURRENCY):{nameof(fxRate)}.{nameof(fxRate.Target)}:({fxRate.Target})"
                ),
                innerException
            )
        )
    {
        Money = money;
        FXRate = fxRate;
    }

    public Money Money { get; }
    public FXRate FXRate { get; }
}

public partial class AppException
{
    public static void ThrowInvalidMoneyFXConvertOperation(
        Money money,
        FXRate fxRate,
        string? message = null,
        Exception? innerException = null
    ) => throw new InvalidMoneyFXConvertOperationException(money, fxRate, message, innerException);
}
