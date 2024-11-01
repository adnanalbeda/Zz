using Microsoft.EntityFrameworkCore;
using Zz.Model;

namespace Zz;

public static partial class TrashPersistenceLinqExtensions
{
    public static int ExecuteSendToTrash<T>(
        this IQueryable<T> query,
        string? reason = null,
        Identity? doneBy = null
    )
        where T : ITrash =>
        query.ExecuteUpdate(e =>
            e.SetProperty(
                x => x.Trashed_,
                new TrashData(UtcNow) { By = doneBy ?? Identity.Unknown, Reason = reason }
            )
        );

    public static int ExecuteRestoreFromTrash<T>(
        this IQueryable<T> query,
        string? reason,
        Identity? doneBy = null
    )
        where T : ITrash =>
        query.ExecuteUpdate(e =>
            e.SetProperty(
                x => x.Trashed_,
                new TrashData(null) { By = doneBy ?? Identity.Unknown, Reason = reason }
            )
        );

    public static int ExecuteRestoreFromTrash<T>(this IQueryable<T> query)
        where T : ITrash<T> =>
        query.ExecuteUpdate(e => e.SetProperty(x => x.Trashed_, TrashData.Available));
}
