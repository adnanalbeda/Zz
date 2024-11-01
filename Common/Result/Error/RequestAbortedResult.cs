using System.Diagnostics.CodeAnalysis;

namespace Zz;

public record struct RequestAbortedResultContent(DateTime At) : IErrorResultContent;

public class RequestAbortedResult()
    : ErrorResult<RequestAbortedResultContent>(
        IResult.StatusCode.Error_RequestAborted,
        new(UtcNow)
    );

public static partial class Result
{
    public static RequestAbortedResult RequestAborted() => new();

    public static Task<RequestAbortedResult> RequestAbortedAsync() =>
        Task.FromResult(RequestAborted());

    public static bool IsRequestAbortedResult(
        this IResult result,
        [NotNullWhen(true)] out RequestAbortedResult? requestAbortedResult
    )
    {
        if (result is RequestAbortedResult res)
        {
            requestAbortedResult = res;
            return true;
        }

        requestAbortedResult = null;
        return false;
    }
}

public static partial class Extensions
{
    public static bool IsAborted(this CancellationToken cancellationToken)
    {
        return cancellationToken.IsCancellationRequested;
    }

    public static bool IsAborted(
        this CancellationToken cancellationToken,
        [NotNullWhen(true)] out IResult? result
    )
    {
        result = cancellationToken.IsCancellationRequested ? Result.RequestAborted() : null;
        return result is not null;
    }

    public static bool IsAborted<T>(
        this CancellationToken cancellationToken,
        [NotNullWhen(true)] out IRequestResult<T>? requestResult
    )
    {
        requestResult = cancellationToken.IsCancellationRequested
            ? Result.RequestAborted().ToReqRes<T>()
            : null;

        return requestResult is not null;
    }
}
