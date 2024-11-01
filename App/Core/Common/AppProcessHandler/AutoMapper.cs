using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Zz.App.Core;

public abstract partial class AppProcessHandler<T>
{
    private IMapper? _Mapper;
    protected IMapper Mapper => _Mapper ??= GetRequiredService<IMapper>();

    protected TD MapTo<TD>(object item) => Mapper.Map<TD>(item);

    protected IQueryable<TD> ProjectQueryTo<TD>(IQueryable queryable) =>
        queryable.ProjectTo<TD>(Mapper.ConfigurationProvider);
}
