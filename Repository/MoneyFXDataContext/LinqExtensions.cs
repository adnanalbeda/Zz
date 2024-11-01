using Microsoft.EntityFrameworkCore;
using Zz.Model;

namespace Zz;

public static partial class MoneyFXRepoExtensions
{
    public static IQueryable<CurrencyBase> CurrencyOptions<T>(this IQueryable<T> currencies)
        where T : CurrencyTable =>
        currencies.Where(x => x.IsSupported).Select(x => new CurrencyBase(x.Code, x.Symbol));

    public static IQueryable<T> LatestFXRate<T>(
        this IQueryable<T> query,
        string source,
        string target
    )
        where T : FXRate, ITrackCreate<T> =>
        query
            .Where(x => x.Source == source && x.Target == target)
            .OrderByTrackCreated(descending: true)
            .Take(1);

    public static IQueryable<T> LatestAnyFXRate<T>(this IQueryable<T> query, string c1, string c2)
        where T : FXRate, ITrackCreate<T> =>
        query
            .Where(x => (x.Source == c1 && x.Target == c2) || (x.Source == c2 && x.Target == c1))
            .OrderByTrackCreated(descending: true)
            .Take(1);
}
