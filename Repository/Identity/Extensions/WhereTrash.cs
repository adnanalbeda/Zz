using Microsoft.EntityFrameworkCore;
using Zz.Model;
using Zz.Model.Identity;

namespace Zz;

public static partial class IdentityPersistenceLinqExtensions
{
    public static IQueryable<T> WhereTrashed<T>(this IQueryable<T> query)
        where T : User => query.Where(x => x.MetaData.Trashed_.Value.HasValue);

    public static IQueryable<T> WhereNotTrashed<T>(this IQueryable<T> query)
        where T : User => query.Where(x => !x.MetaData.Trashed_.Value.HasValue);
}
