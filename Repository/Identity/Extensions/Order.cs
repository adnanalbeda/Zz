using Microsoft.EntityFrameworkCore;
using Zz.Model;
using Zz.Model.Identity;

namespace Zz;

public static partial class IdentityPersistenceLinqExtensions
{
    public static IOrderedQueryable<T> OrderByUserName<T>(
        this IQueryable<T> query,
        bool descending = false
    )
        where T : User =>
        descending ? query.OrderByDescending(x => x.UserName) : query.OrderBy(x => x.UserName);

    public static IOrderedQueryable<T> ThenByUserName<T>(
        this IOrderedQueryable<T> query,
        bool descending = false
    )
        where T : User =>
        descending ? query.OrderByDescending(x => x.UserName) : query.OrderBy(x => x.UserName);
}
