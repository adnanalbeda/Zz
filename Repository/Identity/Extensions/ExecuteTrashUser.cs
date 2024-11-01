using Microsoft.EntityFrameworkCore;
using Zz.Model;
using Zz.Model.Identity;

namespace Zz;

public static partial class IdentityPersistenceLinqExtensions
{
    public static int ExecuteTrashUserProfile<T>(this IQueryable<T> query, Id22 id)
        where T : User =>
        query
            .Where(x => x.Id == id)
            .ExecuteUpdate(e => e.SetProperty(x => x.MetaData.Trashed_, new TrashData(UtcNow)));

    public static int ExecuteRestoreUserProfile<T>(this IQueryable<T> query, Id22 id)
        where T : User =>
        query
            .Where(x => x.Id == id)
            .ExecuteUpdate(e => e.SetProperty(x => x.MetaData.Trashed_, TrashData.Available));
}
