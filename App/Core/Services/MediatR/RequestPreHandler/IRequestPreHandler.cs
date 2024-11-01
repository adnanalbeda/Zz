namespace Zz.App.Core;

using System.Threading;
using System.Threading.Tasks;
using MediatR;

public partial class RequestPreHandler<TReq> : IAppPreRequestHandler<TReq>
{
    public async Task Process(TReq request, CancellationToken cancellationToken)
    {
        await Execute(request, cancellationToken);
    }

    protected abstract Task Execute(TReq request, CancellationToken cancellationToken);
}
