using Microsoft.EntityFrameworkCore;
using Zz.Model;

namespace Zz;

public static partial class TrashPersistenceLinqExtensions
{
    public static Task<int> ExecuteSendToTrashAsync<T>(
        this IQueryable<T> query,
        string? reason = null,
        Identity? doneBy = null
    )
        where T : ITrash<T> =>
        query.ExecuteUpdateAsync(e =>
            e.SetProperty(
                x => x.Trashed_,
                new TrashData(UtcNow) { By = doneBy ?? Identity.Unknown, Reason = reason }
            )
        );

    public static Task<int> ExecuteRestoreFromTrashAsync<T>(
        this IQueryable<T> query,
        string? reason,
        Identity? doneBy = null
    )
        where T : ITrash =>
        query.ExecuteUpdateAsync(e =>
            e.SetProperty(
                x => x.Trashed_,
                new TrashData(null) { By = doneBy ?? Identity.Unknown, Reason = reason }
            )
        );

    public static Task<int> ExecuteRestoreFromTrashAsync<T>(this IQueryable<T> query)
        where T : ITrash =>
        query.ExecuteUpdateAsync(e => e.SetProperty(x => x.Trashed_, TrashData.Available));
}
