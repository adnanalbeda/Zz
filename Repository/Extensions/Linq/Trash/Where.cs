using Zz.Model;

namespace Zz;

public static partial class TrashPersistenceLinqExtensions
{
    public static IQueryable<T> WhereTrashed<T>(this IQueryable<T> query)
        where T : ITrash => query.Where(x => x.Trashed_.Value.HasValue);

    public static IQueryable<T> WhereNotTrashed<T>(this IQueryable<T> query)
        where T : ITrash => query.Where(x => !x.Trashed_.Value.HasValue);
}
