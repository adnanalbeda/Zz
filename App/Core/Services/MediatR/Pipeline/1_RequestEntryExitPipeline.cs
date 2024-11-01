namespace Zz.App.Core;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class RequestEntryExitPipeline<TRequest, TResult>
    : AppProcessHandler<TRequest>,
        IAppRequestPipelineBehavior<TRequest, TResult>
    where TRequest : IAppRequest<TResult>
{
    public RequestEntryExitPipeline(IServiceProvider serviceProvider)
        : base(serviceProvider) { }

    public async Task<IRequestResult<TResult>> Handle(
        TRequest request,
        RequestHandlerDelegate<IRequestResult<TResult>> next,
        CancellationToken cancellationToken
    )
    {
        // BEGIN
        this.AppProcessLogger.ProcessInfo.Started();

        this.AppProcessLogger.LogInformation(
            "REQUEST (ENTRY) :: Processing Started At: ({PROCESS_BEGIN}) :: Next...",
            this.AppProcessLogger.ProcessInfo.StartedAt
        );

        // PROCESS
        var res = await next();

        // END
        this.AppProcessLogger.ProcessInfo.Ended();

        this.AppProcessLogger.LogInformation(
            "REQUEST (EXIT) :: Processing Ended At: ({PROCESS_END}) :: Result ({RSC})",
            this.AppProcessLogger.ProcessInfo.EndedAt,
            res.Result.Status
        );

        return res;
    }
}
