namespace Zz.App.Core;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class ExceptionHandlerPipeline<TRequest, TResult>
    : AppProcessHandler<TRequest>,
        IAppRequestPipelineBehavior<TRequest, TResult>
    where TRequest : IAppRequest<TResult>
{
    public ExceptionHandlerPipeline(IServiceProvider serviceProvider)
        : base(serviceProvider) { }

    public async Task<IRequestResult<TResult>> Handle(
        TRequest request,
        RequestHandlerDelegate<IRequestResult<TResult>> next,
        CancellationToken cancellationToken
    )
    {
        this.AppProcessLogger.LogInformation("REQUEST (EXCEPTION?) :: Next...");
        try
        {
            var res = await next();

            this.AppProcessLogger.LogInformation(
                "REQUEST (EXCEPTION$) :: Completed with no issues."
            );

            return res;
        }
        catch (UnreachableException ex)
        {
            this.AppProcessLogger.LogError(
                ex,
                "REQUEST (EXCEPTION!!) :: CATCH :: UNEXPECTED_VALUE :: THE_END"
            );

            var unexpectedEnd = await Result.UnexpectedEndAsync();
            return await unexpectedEnd.ToReqResAsync<TResult>();
        }
        catch (ArgumentException ex)
        {
            if (ex is ArgumentNullException nex)
                this.AppProcessLogger.LogError(
                    nex,
                    "REQUEST (EXCEPTION!!) :: CATCH :: ARG_NULL_PARAM:{ARG_PARAM} :: THE_END",
                    nex.ParamName
                );
            else if (ex is ArgumentOutOfRangeException oex)
                this.AppProcessLogger.LogError(
                    oex,
                    "REQUEST (EXCEPTION!!) :: CATCH :: ARG_OOR_PARAM:{ARG_PARAM}:{ARG_VALUE} :: THE_END",
                    oex.ParamName,
                    oex.ActualValue
                );
            else
                this.AppProcessLogger.LogError(
                    ex,
                    "REQUEST (EXCEPTION!!) :: CATCH :: ARG_PARAM:{ARG_PARAM} :: THE_END",
                    ex.ParamName
                );

            var errorRes = await Result.InternalErrorAsync<TRequest>(
                ProcessorInfo.Id,
                ex.Message,
                ex
            );
            return await errorRes.ToReqResAsync<TResult>();
        }
        catch (ObjectDisposedException ex)
        {
            this.AppProcessLogger.LogError(
                ex,
                "REQUEST (EXCEPTION!!) :: CATCH :: DISPOSED_OBJECT :: THE_END"
            );

            var errorRes = await Result.InternalErrorAsync<TRequest>(
                ProcessorInfo.Id,
                ex.Message,
                ex
            );
            return await errorRes.ToReqResAsync<TResult>();
        }
        catch (InvalidOperationException ex)
        {
            this.AppProcessLogger.LogError(
                ex,
                "REQUEST (EXCEPTION!!) :: CATCH :: INVALID_OPERATION :: THE_END"
            );

            var errorRes = await Result.InternalErrorAsync<TRequest>(
                ProcessorInfo.Id,
                ex.Message,
                ex
            );
            return await errorRes.ToReqResAsync<TResult>();
        }
        catch (NotImplementedException ex)
        {
#if DEBUG
            this.AppProcessLogger.LogDebug(
                "What's the not implemented {REQ_NAME} which is exposed?",
                typeof(TRequest).FullName
            );
#endif

            this.AppProcessLogger.LogError(
                ex,
                "REQUEST (EXCEPTION!!) :: CATCH :: NOT_IMPLEMENTED :: THE_END"
            );

            var errorRes = await Result.InternalErrorAsync<TRequest>(
                ProcessorInfo.Id,
                ex.Message,
                ex
            );
            return await errorRes.ToReqResAsync<TResult>();
        }
        catch (Exception ex)
        {
            this.AppProcessLogger.LogError(
                ex,
                "REQUEST (EXCEPTION!!) :: CATCH :: EXCEPTION :: THE_END"
            );

            var errorRes = await Result.InternalErrorAsync<TRequest>(
                ProcessorInfo.Id,
                ex.Message,
                ex
            );
            return await errorRes.ToReqResAsync<TResult>();
        }
    }
}
