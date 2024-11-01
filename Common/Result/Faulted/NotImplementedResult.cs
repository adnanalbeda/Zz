using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Zz;

public class NotImplementedResult : FaultedResult
{
    public NotImplementedResult(string message)
        : this(message, null, null) { }

    public NotImplementedResult(string message, Exception? exception)
        : this(message, exception, null) { }

    public NotImplementedResult(string message, ReqInfo? reqInfo)
        : this(message, null, reqInfo) { }

    public NotImplementedResult(
        string message,
        Exception? innerException,
        ReqInfo? reqInfo
    )
        : base(IResult.StatusCode.Failed_NotImplemented, message, innerException, reqInfo) { }
}

public static partial class Result
{
    public static NotImplementedResult NotImplemented(
        string message,
        Exception? exception = null,
        ReqInfo? reqInfo = null
    ) => new(message, exception, reqInfo);

    public static NotImplementedResult NotImplemented<TReq>(
        string reqId,
        string message,
        Exception? exception = null
    )
        where TReq : IProcess =>
        NotImplemented(message, exception, new(reqId, typeof(TReq).FullName ?? typeof(TReq).Name));

    public static Task<NotImplementedResult> NotImplementedAsync(
        string message,
        Exception? exception = null,
        ReqInfo? reqInfo = null
    ) => Task.FromResult(NotImplemented(message, exception, reqInfo));

    public static Task<NotImplementedResult> NotImplementedAsync<TReq>(
        string reqId,
        string message,
        Exception? exception = null
    )
        where TReq : IProcess =>
        NotImplementedAsync(
            message,
            exception,
            new(reqId, typeof(TReq).FullName ?? typeof(TReq).Name)
        );

    public static Task<IErrorResult> NotImplementedAsyncResult(
        string message,
        Exception? exception = null,
        ReqInfo? reqInfo = null
    ) => Task.FromResult<IErrorResult>(NotImplemented(message, exception, reqInfo));

    public static Task<IErrorResult> NotImplementedAsyncResult<TReq>(
        string reqId,
        string message,
        Exception? exception = null
    )
        where TReq : IProcess =>
        NotImplementedAsyncResult(
            message,
            exception,
            new(reqId, typeof(TReq).FullName ?? typeof(TReq).Name)
        );

    public static bool IsNotImplementedResult(
        this IResult result,
        [NotNullWhen(true)] out NotImplementedResult? notImplementedResult
    )
    {
        if (result is NotImplementedResult res)
        {
            notImplementedResult = res;
            return true;
        }

        notImplementedResult = null;
        return false;
    }
}
