using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Zz;

public class FaultedResult : IErrorResult<ResultException>
{
    public FaultedResult(IResult.StatusCode statusCode, string message)
        : this(statusCode, message, null, null) { }

    public FaultedResult(IResult.StatusCode statusCode, string message, Exception? exception)
        : this(statusCode, message, exception, null) { }

    public FaultedResult(IResult.StatusCode statusCode, string message, ReqInfo? reqInfo)
        : this(statusCode, message, null, reqInfo) { }

    public FaultedResult(
        IResult.StatusCode statusCode,
        string message,
        Exception? innerException,
        ReqInfo? reqInfo
    )
    {
        if (!statusCode.IsFailure())
            throw new UnreachableException("'statusCode' is not a failure error code.");

        this.Status = statusCode;
        this.Content = new(new(message, innerException, reqInfo));
    }

    public IResult.StatusCode Status { get; }

    public ContentWrapper<ResultException> Content { get; }
}

public static partial class Result
{
    public static FaultedResult Faulted(
        IResult.StatusCode statusCode,
        string message,
        Exception? exception = null,
        ReqInfo? reqInfo = null
    ) => new(statusCode, message, exception, reqInfo);

    public static FaultedResult Faulted<TReq>(
        string reqId,
        IResult.StatusCode statusCode,
        string message,
        Exception? exception = null
    )
        where TReq : IProcess =>
        Faulted(
            statusCode,
            message,
            exception,
            new(reqId, typeof(TReq).FullName ?? typeof(TReq).Name)
        );

    public static Task<FaultedResult> FaultedAsync(
        IResult.StatusCode statusCode,
        string message,
        Exception? exception = null,
        ReqInfo? reqInfo = null
    ) => Task.FromResult(Faulted(statusCode, message, exception, reqInfo));

    public static Task<FaultedResult> FaultedAsync<TReq>(
        string reqId,
        IResult.StatusCode statusCode,
        string message,
        Exception? exception = null
    )
        where TReq : IProcess =>
        FaultedAsync(
            statusCode,
            message,
            exception,
            new(reqId, typeof(TReq).FullName ?? typeof(TReq).Name)
        );

    public static bool IsFaultedResult(
        this IResult result,
        [NotNullWhen(true)] out FaultedResult? faultedResult
    )
    {
        if (result is FaultedResult res)
        {
            faultedResult = res;
            return true;
        }

        faultedResult = null;
        return false;
    }
}
