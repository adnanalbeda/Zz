using System.Diagnostics.CodeAnalysis;

namespace Zz;

public class InternalErrorResult : FaultedResult
{
    public InternalErrorResult(string message, Exception innerException, ReqInfo? reqInfo = null)
        : base(IResult.StatusCode.Failed_InternalError, message, innerException, reqInfo) { }
}

public static partial class Result
{
    public static InternalErrorResult InternalError(
        string message,
        Exception exception,
        ReqInfo? reqInfo = null
    ) => new(message, exception, reqInfo);

    public static InternalErrorResult InternalError<TReq>(
        string reqId,
        string message,
        Exception exception
    )
        where TReq : IProcess =>
        InternalError(message, exception, new(reqId, typeof(TReq).FullName ?? typeof(TReq).Name));

    public static Task<InternalErrorResult> InternalErrorAsync(
        string message,
        Exception exception,
        ReqInfo? reqInfo = null
    ) => Task.FromResult(InternalError(message, exception, reqInfo));

    public static Task<InternalErrorResult> InternalErrorAsync<TReq>(
        string reqId,
        string message,
        Exception exception
    )
        where TReq : IProcess =>
        InternalErrorAsync(
            message,
            exception,
            new(reqId, typeof(TReq).FullName ?? typeof(TReq).Name)
        );

    public static bool IsInternalErrorResult(
        this IResult result,
        [NotNullWhen(true)] out InternalErrorResult? internalErrorResult
    )
    {
        if (result is InternalErrorResult res)
        {
            internalErrorResult = res;
            return true;
        }

        internalErrorResult = null;
        return false;
    }
}
