using Zz.Model;

namespace Zz;

public static partial class TrackPersistenceLinqExtensions
{
    public static IOrderedQueryable<T> OrderByTrackUpdated<T>(
        this IQueryable<T> query,
        bool descending = true
    )
        where T : ITrackUpdate =>
        descending
            ? query.OrderByDescending(x => x.Track_UpdatedAt_)
            : query.OrderBy(x => x.Track_UpdatedAt_);

    public static IOrderedQueryable<T> ThenByTrackUpdated<T>(
        this IOrderedQueryable<T> query,
        bool descending = true
    )
        where T : ITrackUpdate =>
        descending
            ? query.ThenByDescending(x => x.Track_UpdatedAt_)
            : query.ThenBy(x => x.Track_UpdatedAt_);
}
