using Zz.Model;

namespace Zz;

public static partial class TrackPersistenceLinqExtensions
{
    public static IOrderedQueryable<T> OrderByTrackCreated<T>(
        this IQueryable<T> query,
        bool descending = true
    )
        where T : ITrackCreate =>
        descending
            ? query.OrderByDescending(x => x.Track_CreatedAt_)
            : query.OrderBy(x => x.Track_CreatedAt_);

    public static IOrderedQueryable<T> ThenByTrackCreated<T>(
        this IOrderedQueryable<T> query,
        bool descending = true
    )
        where T : ITrackCreate =>
        descending
            ? query.ThenByDescending(x => x.Track_CreatedAt_)
            : query.ThenBy(x => x.Track_CreatedAt_);
}
