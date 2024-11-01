namespace Zz.App.Core;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zz;

public partial class RequestPostHandler<TReq, TRes> : IAppPostRequestHandler<TReq, TRes>
{
    public async Task Process(
        TReq request,
        IRequestResult<TRes> response,
        CancellationToken cancellationToken
    )
    {
        await Execute(request, response, cancellationToken);
    }

    protected abstract Task Execute(
        TReq request,
        IRequestResult<TRes> response,
        CancellationToken cancellationToken
    );
}
