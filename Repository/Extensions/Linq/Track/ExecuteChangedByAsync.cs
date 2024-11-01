using Microsoft.EntityFrameworkCore;
using Zz.Model;

namespace Zz;

public static partial class TrackPersistenceLinqExtensions
{
    public static Task<int> ExecuteTrackChangedByAsync<T>(this IQueryable<T> query, Guid id)
        where T : ITrackChangeDoer<T> =>
        query.ExecuteTrackChangedByAsync(IdentityKind.Guid, id.ToString());

    public static Task<int> ExecuteTrackChangedByAsync<T>(
        this IQueryable<T> query,
        IdentityKind idKind,
        string value
    )
        where T : ITrackChangeDoer<T> =>
        query.ExecuteTrackChangedByAsync(new Identity(idKind, value));

    public static Task<int> ExecuteTrackChangedByAsync<T>(this IQueryable<T> query, Identity doer)
        where T : ITrackChangeDoer<T> =>
        query.ExecuteUpdateAsync(e =>
            e.SetProperty(x => x.Track_UpdatedAt_, UtcNow).SetProperty(x => x.Track_DoneBy_, doer)
        );
}
