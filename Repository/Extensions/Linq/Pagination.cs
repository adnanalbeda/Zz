namespace Zz;

using Microsoft.EntityFrameworkCore;
using Zz.Pagination;

public static class PaginationQueryExtensions
{
    public static IPaginationMeta NewPaginationMeta<T>(
        this IQueryable<T> query,
        IPagingParams pagingParams
    )
    {
        return Pagination.Pagination.GenerateMeta(pagingParams, query.Count());
    }

    public static async Task<IPaginationMeta> NewPaginationMetaAsync<T>(
        this IQueryable<T> query,
        IPagingParams pagingParams
    )
    {
        return Pagination.Pagination.GenerateMeta(pagingParams, await query.CountAsync());
    }

    public static IPagination<T> Paged<T>(this IQueryable<T> query, IPaginationMeta pagedMeta)
        where T : class
    {
        return Pagination.Pagination.New(
            query.Skip(pagedMeta.SkipItemsCount()).Take(pagedMeta.PageSize),
            pagedMeta
        );
    }

    public static IPagination<T> Paged<T>(this IQueryable<T> query, IPagingParams pagingParams)
        where T : class
    {
        return Pagination.Pagination.New(
            query.Skip(pagingParams.SkipItemsCount()).Take(pagingParams.PageSize),
            pagingParams,
            query.Count()
        );
    }

    public static async Task<IPagination<T>> PagedAsync<T>(
        this IQueryable<T> query,
        Task<IPaginationMeta> pagedMeta
    )
        where T : class => await PagedAsync(query, await pagedMeta);

    public static async Task<IPagination<T>> PagedAsync<T>(
        this IQueryable<T> query,
        IPagingParams pagingParams
    )
        where T : class
    {
        return Pagination.Pagination.New(
            query.Skip(pagingParams.SkipItemsCount()).Take(pagingParams.PageSize),
            pagingParams,
            await query.CountAsync()
        );
    }
}
