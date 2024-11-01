namespace Zz.App.Core;

using System;
using System.Threading;
using System.Threading.Tasks;
using Zz;
using Zz.Pagination;

public abstract partial class PaginationRequestHandler<TReq, TRes>(IServiceProvider serviceProvider)
    : RequestHandler<TReq, IPagination<TRes>>(serviceProvider)
    where TReq : IPagedQuery<TRes>
    where TRes : class
{
    protected sealed override async Task<IRequestResult<IPagination<TRes>>> Execute(
        TReq req,
        CancellationToken cancellationToken
    )
    {
        var pq = await RequestQuery(req, cancellationToken);
        var res = await Result.OkAsync(await pq.Query.PagedAsync(pq.LoadedMeta));
        return await res.ToReqResAsync();
    }

    protected abstract Task<(IQueryable<TRes> Query, IPaginationMeta LoadedMeta)> RequestQuery(
        TReq req,
        CancellationToken cancellationToken
    );
}
