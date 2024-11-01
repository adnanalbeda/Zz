namespace Zz.Pagination;

public interface IPaginationMeta : IPagingParams
{
    public uint TotalItemsCount { get; }

    public uint TotalPages() => this.TotalItemsCount / this.PageSize;

    public bool HasMoreNext() => this.PageNumber < TotalPages();

    public bool HasMoreBefore() => this.PageNumber > 1 && this.TotalItemsCount > this.PageSize;
}

internal sealed class PaginationMeta : PagingParams, IPaginationMeta
{
    internal PaginationMeta(IPagingParams pagingParams, uint totalItemsCount)
        : base(pagingParams)
    {
        this.TotalItemsCount = totalItemsCount;
    }

    public uint TotalItemsCount { get; }
}

public static partial class Pagination
{
    public static IPaginationMeta GenerateMeta(IPagingParams pagingParams, uint totalItemsCount) =>
        new PaginationMeta(pagingParams, totalItemsCount);

    public static IPaginationMeta GenerateMeta(IPagingParams pagingParams, long totalItemsCount) =>
        new PaginationMeta(pagingParams, totalItemsCount < 0 ? 0 : (uint)totalItemsCount);
}
