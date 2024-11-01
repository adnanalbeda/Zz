using AutoMapper;
using AutoMapper.QueryableExtensions;
using Zz.Pagination;

namespace Zz;

public static class AutoMapperQueryExtensions
{
    public static IPagination<T> ProjectPagination<T, TSource>(
        this IMapper mapper,
        IQueryable<TSource> query,
        IPagingParams pagingParams
    )
        where T : class
        where TSource : class
    {
        var meta = query.NewPaginationMeta(pagingParams);
        return query.ProjectTo<T>(mapper.ConfigurationProvider).Paged(meta);
    }

    public static Task<IPagination<T>> ProjectPaginationAsync<T, TSource>(
        this IMapper mapper,
        IQueryable<TSource> query,
        IPagingParams pagingParams
    )
        where T : class
        where TSource : class
    {
        var metaAsync = query.NewPaginationMetaAsync(pagingParams);
        return query.ProjectTo<T>(mapper.ConfigurationProvider).PagedAsync(metaAsync);
    }
}
