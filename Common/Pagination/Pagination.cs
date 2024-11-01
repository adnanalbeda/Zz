namespace Zz.Pagination;

public interface IPagination<out T> : IPaginationValues<T>
{
    public IPaginationMeta Meta { get; }
}

internal sealed class Pagination<T> : IPagination<T>
{
    public IPaginationMeta Meta { get; }
    public IEnumerable<T> Values { get; }

    public Pagination(IEnumerable<T> values, IPaginationMeta paginationMeta)
    {
        this.Meta = paginationMeta;
        this.Values = values;
    }
}

public static partial class Pagination
{
    public static IPagination<T> New<T>(
        IEnumerable<T> values,
        IPagingParams pagingParams,
        uint totalItemsCount
    ) => new Pagination<T>(values, GenerateMeta(pagingParams, totalItemsCount));

    public static IPagination<T> New<T>(
        IEnumerable<T> values,
        IPagingParams pagingParams,
        long totalItemsCount
    ) => new Pagination<T>(values, GenerateMeta(pagingParams, totalItemsCount));

    public static IPagination<T> New<T>(IEnumerable<T> values, IPaginationMeta paginationMeta) =>
        new Pagination<T>(values, paginationMeta);
}
