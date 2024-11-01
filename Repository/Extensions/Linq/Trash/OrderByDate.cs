using Zz.Model;

namespace Zz;

public static partial class TrashPersistenceLinqExtensions
{
    public static IOrderedQueryable<T> OrderByTrashDate<T>(
        this IQueryable<T> query,
        bool descending = true
    )
        where T : ITrash =>
        descending
            ? query.OrderByDescending(x => x.Trashed_.Value)
            : query.OrderBy(x => x.Trashed_.Value);

    public static IOrderedQueryable<T> ThenByTrashDate<T>(
        this IOrderedQueryable<T> query,
        bool descending = true
    )
        where T : ITrash =>
        descending
            ? query.OrderByDescending(x => x.Trashed_.Value)
            : query.OrderBy(x => x.Trashed_.Value);
}
