namespace Zz.App.Core;

using System.Threading;
using System.Threading.Tasks;
using MediatR;

public partial class RequestHandler<TReq, TRes> : IAppRequestHandler<TReq, TRes>
{
    public Task<IRequestResult<TRes>> Handle(TReq request, CancellationToken cancellationToken)
    {
        this.AppProcessLogger.LogInformation("REQUEST :: Handling...");
        return Execute(request, cancellationToken);
    }

    protected abstract Task<IRequestResult<TRes>> Execute(
        TReq req,
        CancellationToken cancellationToken
    );
}
