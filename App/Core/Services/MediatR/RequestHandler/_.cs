namespace Zz.App.Core;

using System;
using Microsoft.AspNetCore.Http;

public abstract partial class RequestHandler<TReq, TRes>(IServiceProvider serviceProvider)
    : AppProcessHandler<TReq>(serviceProvider)
    where TReq : IAppRequest<TRes>
{
    private HttpContext? httpContext;
    protected HttpContext HttpContext =>
        (httpContext ??= GetRequiredService<IHttpContextAccessor>().HttpContext!);

    protected async Task<IRequestResult<TRes>> SUCCESS_OK(TRes res)
    {
#if DEBUG
        this.AppProcessLogger.LogTrace("Request handled successfully.");
#endif

        return await Result.OkAsyncResult(res).ToReqResAsync();
    }

    protected async Task<IRequestResult<Null>> SUCCESS_EMPTY()
    {
#if DEBUG
        this.AppProcessLogger.LogTrace("Request handled successfully.");
#endif

        return await (await Result.EmptyAsync).ToReqResAsync();
    }

    protected async Task<IRequestResult<TRes>> NOT_FOUND(
        string message = Result.NOT_FOUND_STRING,
        string code = Result.NOT_FOUND_STRING
    )
    {
#if DEBUG
        this.AppProcessLogger.LogTrace("NOT_FOUND Result. ({MESSAGE}-{CODE})", message, code);
#endif

        return await Result.NotFoundAsyncResult(message, code).ToReqResAsync<TRes>();
    }

    protected async Task<IRequestResult<TRes>> UNPROCESSABLE(
        string message,
        string? code = null,
        IEnumerable<InvalidRequestResultContent.Error>? errors = null
    )
    {
#if DEBUG
        this.AppProcessLogger.LogTrace(
            "Request unprocessable. ({MESSAGE} - {CODE})",
            message,
            code
        );
#endif

        return await Result
            .UnprocessableRequestAsyncResult(message, code, errors)
            .ToReqResAsync<TRes>();
    }

    protected async Task<IRequestResult<TRes>> UNEXPECTED_END_OF_PROCESS()
    {
#if DEBUG
        this.AppProcessLogger.LogTrace(
            "Request reached unexpected and unprocessable end. ({REQ_NAME})",
            typeof(TReq).FullName
        );
#endif

        return await Result
            .UnexpectedEndAsyncResult<TReq>(this.ProcessorInfo.Id)
            .ToReqResAsync<TRes>();
    }

    protected async Task<IRequestResult<TRes>> WRONG_CONTEXT()
    {
#if DEBUG
        this.AppProcessLogger.LogTrace("WRONG_CONTEXT Result.");
#endif

        return await Result.WrongContextAsyncResult().ToReqResAsync<TRes>();
    }

    protected async Task<IRequestResult<TRes>> ABORTED()
    {
#if DEBUG
        this.AppProcessLogger.LogTrace("WRONG_CONTEXT Result.");
#endif

        return await (await Result.RequestAbortedAsync()).ToReqResAsync<TRes>();
    }
}
