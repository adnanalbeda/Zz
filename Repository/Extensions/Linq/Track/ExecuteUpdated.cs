using Microsoft.EntityFrameworkCore;
using Zz.Model;

namespace Zz;

public static partial class TrackPersistenceLinqExtensions
{
    public static int ExecuteTrackUpdated<T>(this IQueryable<T> query)
        where T : ITrackUpdate<T> =>
        query.ExecuteUpdate(e => e.SetProperty(x => x.Track_UpdatedAt_, UtcNow));
}
