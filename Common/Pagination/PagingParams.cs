namespace Zz.Pagination;

public interface IPagingParams
{
    public int PageNumber { get; }
    public byte PageSize { get; }

    public int SkipItemsCount() => (PageNumber - 1) * PageSize;
}

public class PagingParams : IPagingParams
{
    public int PageNumber { get; }
    public byte PageSize { get; }

    public PagingParams(int pageNumber, byte pageSize = 12)
    {
        if (pageNumber < 1)
            pageNumber = 1;
        if (pageSize < 1)
            pageSize = 1;

        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
    }

    protected PagingParams(IPagingParams pagingParams)
        : this(pagingParams.PageNumber, pagingParams.PageSize) { }
}
