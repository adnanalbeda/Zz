namespace Zz.App.Core;

using System.Threading;
using System.Threading.Tasks;
using Zz;

public partial class RequestPipelineHandler<TReq, TRes> : IAppPostRequestHandler<TReq, TRes>
{
    public async Task Process(
        TReq request,
        IRequestResult<TRes> response,
        CancellationToken cancellationToken
    )
    {
        await PostExecute(request, response, cancellationToken);
    }

    protected abstract Task PostExecute(
        TReq request,
        IRequestResult<TRes> response,
        CancellationToken cancellationToken
    );
}
