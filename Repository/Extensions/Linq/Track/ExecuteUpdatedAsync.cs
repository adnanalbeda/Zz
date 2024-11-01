using Microsoft.EntityFrameworkCore;
using Zz.Model;

namespace Zz;

public static partial class TrackPersistenceLinqExtensions
{
    public static Task<int> ExecuteTrackUpdatedAsync<T>(this IQueryable<T> query)
        where T : ITrackUpdate<T> =>
        query.ExecuteUpdateAsync(e => e.SetProperty(x => x.Track_UpdatedAt_, UtcNow));
}
