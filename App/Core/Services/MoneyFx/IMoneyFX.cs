namespace Zz.App.Core;

/****
* Since Money Currency can be optionally provided for default amount value,
* Guarding against that is a must do.
****/

public interface IMoneyFX
{
    public Money Convert(Money money, FXRate fxRate)
    {
        AppArgumentException.ThrowIfInvalidCurrencyFormat(money.Currency);

        if (money.Currency == fxRate.Source)
            return new(money.Amount * fxRate.Rates, fxRate.Target);

        if (money.Currency == fxRate.Target)
            return new(money.Amount / fxRate.Rates, fxRate.Source);

        throw new InvalidMoneyFXConvertOperationException(
            money,
            fxRate,
            innerException: new ArgumentException(
                $"Wrong {nameof(fxRate)} is provided for {nameof(money)}.",
                nameof(fxRate)
            )
        );
    }

    public Money ConvertTo(Money money, string targetCurrency)
    {
        AppArgumentException.ThrowIfInvalidCurrencyFormat(money.Currency);
        AppArgumentException.ThrowIfInvalidCurrencyFormat(targetCurrency);

        if (money.Currency == targetCurrency)
            return money;

        var fx = LoadLastFxRate(money.Currency, targetCurrency);

        var res = ConvertToTarget_(money, fx);

        return res;
    }
    public Money ConvertTo(Money money, FXRate fxRate)
    {
        AppArgumentException.ThrowIfInvalidCurrencyFormat(money.Currency);

        var res = ConvertToTarget_(money, fxRate);

        return res;
    }
    private Money ConvertToTarget_(Money money, FXRate fxRate)
    {
        AppArgumentException.ThrowIfInvalidCurrencyFormat(money.Currency);

        if (money.Currency == fxRate.Source)
            return new(money.Amount * fxRate.Rates, fxRate.Target);

        throw new InvalidMoneyFXConvertOperationException(
            money,
            fxRate,
            innerException: new ArgumentException(
                string.Concat(
                    $"Wrong ({nameof(fxRate)}) is provided for ({nameof(money)}).",
                    Environment.NewLine,
                    $"({nameof(fxRate)}.{nameof(fxRate.Source)}):({fxRate.Source}) currency must be the same of provided ({nameof(money)}.{nameof(money.Currency)}):({money.Currency})."
                ),
                nameof(fxRate)
            )
        );
    }

    public FXRate LoadLastFxRate(string source, string target)
    {
        AppArgumentException.ThrowIfInvalidCurrencyFormat(source);

        AppArgumentException.ThrowIfInvalidCurrencyFormat(target);

        if (source == target)
            return new(source, target, 1);

        var fxFromExternal = LoadLastFxRateFromExternalSource(source, target);

        return fxFromExternal;
    }
    protected FXRate LoadLastFxRateFromExternalSource(string sourceCurrency, string targetCurrency);

    public FXRate LoadAnyLastFxRate(string c1, string c2)
    {
        AppArgumentException.ThrowIfInvalidCurrencyFormat(c1);

        AppArgumentException.ThrowIfInvalidCurrencyFormat(c2);

        if (c1 == c2)
            return new(c1, c2, 1);

        var fxFromExternal = LoadAnyLastFxRateFromExternalSource(c1, c2);

        return fxFromExternal;
    }
    protected FXRate LoadAnyLastFxRateFromExternalSource(string c1, string c2);
}
