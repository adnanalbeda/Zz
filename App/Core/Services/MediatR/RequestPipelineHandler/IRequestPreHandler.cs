namespace Zz.App.Core;

using System.Threading;
using System.Threading.Tasks;
using MediatR;

public partial class RequestPipelineHandler<TReq, TRes> : IAppPreRequestHandler<TReq>
{
    public async Task Process(TReq request, CancellationToken cancellationToken)
    {
        await PreExecute(request, cancellationToken);
    }

    protected abstract Task PreExecute(TReq request, CancellationToken cancellationToken);
}
