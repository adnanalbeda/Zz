using Microsoft.EntityFrameworkCore;
using Zz.Model;

namespace Zz;

public static partial class MoneyFXRepoExtensions
{
    public static int ExecuteUpdateCurrencySupport<T>(
        this IQueryable<T> currencies,
        bool isSupported,
        Identity? doneBy
    )
        where T : Currency =>
        currencies.ExecuteUpdate(x =>
            x.SetProperty(x => x.IsSupported, isSupported)
                .SetProperty(x => x.Track_UpdatedAt_, UtcNow)
                .SetProperty(x => x.Track_DoneBy_, doneBy ?? Identity.Unknown)
        );

    public static Task<int> ExecuteUpdateCurrencySupportAsync<T>(
        this IQueryable<T> currencies,
        bool isSupported,
        Identity? doneBy
    )
        where T : Currency =>
        currencies.ExecuteUpdateAsync(x =>
            x.SetProperty(x => x.IsSupported, isSupported)
                .SetProperty(x => x.Track_UpdatedAt_, UtcNow)
                .SetProperty(x => x.Track_DoneBy_, doneBy ?? Identity.Unknown)
        );

    public static int UpdateCurrencySupport<T>(
        this IQueryable<T> currencies,
        int id,
        bool isSupported,
        Identity? doneBy
    )
        where T : Currency =>
        currencies.Where(x => x.Id == id).ExecuteUpdateCurrencySupport(isSupported, doneBy);

    public static int UpdateCurrencySupport<T>(
        this IQueryable<T> currencies,
        string code,
        bool isSupported,
        Identity? doneBy
    )
        where T : Currency =>
        currencies.Where(x => x.Code == code).ExecuteUpdateCurrencySupport(isSupported, doneBy);

    public static Task<int> UpdateCurrencySupportAsync<T>(
        this IQueryable<T> currencies,
        int id,
        bool isSupported,
        Identity? doneBy
    )
        where T : Currency =>
        currencies.Where(x => x.Id == id).ExecuteUpdateCurrencySupportAsync(isSupported, doneBy);

    public static Task<int> UpdateCurrencySupportAsync<T>(
        this IQueryable<T> currencies,
        string code,
        bool isSupported,
        Identity? doneBy
    )
        where T : Currency =>
        currencies
            .Where(x => x.Code == code)
            .ExecuteUpdateCurrencySupportAsync(isSupported, doneBy);
}
