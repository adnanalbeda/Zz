using System.Linq;
using Zz.App.Core;
using Zz.DataBase;

namespace Zz.Services;

public class MoneyFX(MoneyFXDataContext fxDataContext) : IMoneyFX
{
    private readonly MoneyFXDataContext _FXDataContext = fxDataContext;

    FXRate IMoneyFX.LoadAnyLastFxRateFromExternalSource(string c1, string c2)
    {
        var result = _FXDataContext.MoneyFXRates.LatestAnyFXRate(c1, c2).FirstOrDefault();

        if (result is not null && result.Track_CreatedAt_ > UtcNow.Add(TimeSpan.FromHours(-10)))
            return result;

        return OldResultOrNoResultFound(c1, c2);
    }

    FXRate IMoneyFX.LoadLastFxRateFromExternalSource(string sourceCurrency, string targetCurrency)
    {
        var result = _FXDataContext
            .MoneyFXRates.LatestFXRate(sourceCurrency, targetCurrency)
            .FirstOrDefault();

        if (result is not null && result.Track_CreatedAt_ > UtcNow.Add(TimeSpan.FromHours(-10)))
            return result;

        return OldResultOrNoResultFound(sourceCurrency, targetCurrency);
    }

    private FXRate OldResultOrNoResultFound(string sourceCurrency, string targetCurrency)
    {
        var areSupported = _FXDataContext
            .Currencies.Where(x => NewEnumerable(sourceCurrency, targetCurrency).Contains(x.Code))
            .All(x => x.IsSupported);

        if (!areSupported)
            throw new InvalidMoneyOperationException(
                sourceCurrency,
                targetCurrency,
                "(One/Both) of the currencies (is/are) not supported!"
            );

        return UpdateFromExternalApi(sourceCurrency, targetCurrency);
    }

    private FXRate UpdateFromExternalApi(string sourceCurrency, string targetCurrency)
    {
        throw new NotImplementedException();
    }
}
