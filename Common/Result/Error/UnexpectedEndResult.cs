using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Zz;

public record UnexpectedEndResultContent(string Message, ReqInfo? ReqInfo) : IErrorResultContent;

public class UnexpectedEndResult : ErrorResult<UnexpectedEndResultContent>
{
    public UnexpectedEndResult(ReqInfo? reqInfo = null)
        : base(
            IResult.StatusCode.Error_UnexpectedEndOfProcess,
            new("Reached end of process without a valid result to return.", reqInfo)
        ) { }
}

public static partial class Result
{
    public static UnexpectedEndResult UnexpectedEnd(ReqInfo? reqInfo = null) =>
        new UnexpectedEndResult(reqInfo);

    public static UnexpectedEndResult UnexpectedEnd<TReq>(string reqId)
        where TReq : IProcess =>
        UnexpectedEnd(new(reqId, typeof(TReq).FullName ?? typeof(TReq).Name));

    public static Task<UnexpectedEndResult> UnexpectedEndAsync(ReqInfo? reqInfo = null) =>
        Task.FromResult(UnexpectedEnd(reqInfo));

    public static Task<UnexpectedEndResult> UnexpectedEndAsync<TReq>(string reqId)
        where TReq : IProcess => Task.FromResult(UnexpectedEnd<TReq>(reqId));

    public static Task<IErrorResult> UnexpectedEndAsyncResult(ReqInfo? reqInfo = null) =>
        Task.FromResult((IErrorResult)UnexpectedEnd(reqInfo));

    public static Task<IErrorResult> UnexpectedEndAsyncResult<TReq>(string reqId)
        where TReq : IProcess => Task.FromResult((IErrorResult)UnexpectedEnd<TReq>(reqId));
}
