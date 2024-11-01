using System.Diagnostics.CodeAnalysis;

namespace Zz;

public record struct TooManyRequestsResultContent(
    DateTime? AvailableAfter,
    string? Message,
    string? Code
) : IErrorResultContent;

/// <summary>
/// How is this client fault?<br/>
/// They shouldn't send too many requests. So sus.
/// </summary>
public class TooManyRequestsResult(DateTime? availableAfter, string? message, string? code)
    : ErrorResult<TooManyRequestsResultContent>(
        IResult.StatusCode.Error_TooManyRequests,
        new(availableAfter?.ToUniversalTime(), message, code)
    );

public static partial class Result
{
    public const string TOO_MANY_REQUESTS_MESSAGE = "Too many requests. Try again later!";
    public const string TOO_MANY_REQUESTS_CODE = "TOO_MANY_REQ";

    public static TooManyRequestsResult TooManyRequests(
        DateTime? availableAfter = null,
        string? message = TOO_MANY_REQUESTS_MESSAGE,
        string? code = TOO_MANY_REQUESTS_CODE
    ) => new(availableAfter, message, code);

    public static Task<TooManyRequestsResult> TooManyRequestsAsync(
        DateTime? availableAfter = null,
        string? message = TOO_MANY_REQUESTS_MESSAGE,
        string? code = TOO_MANY_REQUESTS_CODE
    ) => Task.FromResult(TooManyRequests(availableAfter, message, code));

    public static Task<IErrorResult> TooManyRequestsAsyncResult(
        DateTime? availableAfter = null,
        string? message = TOO_MANY_REQUESTS_MESSAGE,
        string? code = TOO_MANY_REQUESTS_CODE
    ) => Task.FromResult<IErrorResult>(TooManyRequests(availableAfter, message, code));

    public static bool IsTooManyRequestsResult(
        this IResult result,
        [NotNullWhen(true)] out TooManyRequestsResult? tooManyRequestsResult
    )
    {
        if (result is TooManyRequestsResult res)
        {
            tooManyRequestsResult = res;
            return true;
        }

        tooManyRequestsResult = null;
        return false;
    }
}
