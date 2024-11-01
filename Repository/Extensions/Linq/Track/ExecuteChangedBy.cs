using Microsoft.EntityFrameworkCore;
using Zz.Model;

namespace Zz;

public static partial class TrackPersistenceLinqExtensions
{
    public static int ExecuteTrackChangedBy<T>(this IQueryable<T> query, Guid id)
        where T : ITrackChangeDoer<T> =>
        query.ExecuteTrackChangedBy(IdentityKind.Guid, id.ToString());

    public static int ExecuteTrackChangedBy<T>(
        this IQueryable<T> query,
        IdentityKind idKind,
        string value
    )
        where T : ITrackChangeDoer<T> => query.ExecuteTrackChangedBy(new Identity(idKind, value));

    public static int ExecuteTrackChangedBy<T>(this IQueryable<T> query, Identity doer)
        where T : ITrackChangeDoer<T> =>
        query.ExecuteUpdate(e =>
            e.SetProperty(x => x.Track_UpdatedAt_, UtcNow).SetProperty(x => x.Track_DoneBy_, doer)
        );
}
