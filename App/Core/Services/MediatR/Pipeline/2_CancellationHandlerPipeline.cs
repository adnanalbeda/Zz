namespace Zz.App.Core;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class CancellationHandlerPipeline<TRequest, TResult>
    : AppProcessHandler<TRequest>,
        IAppRequestPipelineBehavior<TRequest, TResult>
    where TRequest : IAppRequest<TResult>
{
    public CancellationHandlerPipeline(IServiceProvider serviceProvider)
        : base(serviceProvider) { }

    public async Task<IRequestResult<TResult>> Handle(
        TRequest request,
        RequestHandlerDelegate<IRequestResult<TResult>> next,
        CancellationToken cancellationToken
    )
    {
        this.AppProcessLogger.LogInformation("REQUEST (CANCELLATION?) :: Next...");

        try
        {
            var res = await next();

            this.AppProcessLogger.LogInformation(
                "REQUEST (CANCELLATION$) :: Completed with no issues."
            );

            return res;
        }
        catch (OperationCanceledException)
        {
            if (request is not IQuery)
                this.AppProcessLogger.LogWarning(
                    "REQUEST (CANCELLATION!!!) :: CANCELLED AT: ({CNCL_AT}).",
                    UtcNow
                );
            else
                this.AppProcessLogger.LogInformation(
                    "REQUEST (CANCELLATION!) :: CANCELLED AT: ({CNCL_AT}).",
                    UtcNow
                );

            var abortedRes = await Result.RequestAbortedAsync();

            return await abortedRes.ToReqResAsync<TResult>();
        }
    }
}
