using Microsoft.EntityFrameworkCore;
using Zz.DataBase;

namespace Zz.App.Core;

public abstract partial class AppProcessHandler<T>
{
    private DataContext? _DataContext;
    protected internal DataContext DataContext =>
        _DataContext ??= GetRequiredService<DataContext>();

    protected IQueryable<TData> Query<TData>()
        where TData : class => DataContext.Set<TData>().AsQueryable();

    protected IQueryable<TData> NoTrackingQuery<TData>()
        where TData : class => Query<TData>().AsNoTracking();

    protected int SaveDataChanges() => DataContext.SaveChanges();

    protected Task<int> SaveDataChangesAsync() => DataContext.SaveChangesAsync();
}
