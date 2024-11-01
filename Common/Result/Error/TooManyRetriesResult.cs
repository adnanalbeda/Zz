using System.Diagnostics.CodeAnalysis;

namespace Zz;

public record struct TooManyRetriesResultContent(
    DateTime? AvailableAfter,
    string? Message,
    string? Code
) : IErrorResultContent;

/// <summary>
/// How is this client fault?<br/>
/// They shouldn't send too many requests. So sus.
/// </summary>
public class TooManyRetriesResult(DateTime? availableAfter, string? message, string? code)
    : ErrorResult<TooManyRetriesResultContent>(
        IResult.StatusCode.Error_TooManyRetries,
        new(availableAfter?.ToUniversalTime(), message, code)
    );

public static partial class Result
{
    public const string TOO_MANY_RETRIES_MESSAGE = "Too many retries. Try again later!";
    public const string TOO_MANY_RETRIES_CODE = "TOO_MANY_RETRIES";

    public static TooManyRetriesResult TooManyRetries(
        DateTime? availableAfter = null,
        string? message = TOO_MANY_RETRIES_MESSAGE,
        string? code = TOO_MANY_RETRIES_CODE
    ) => new(availableAfter, message, code);

    public static Task<TooManyRetriesResult> TooManyRetriesAsync(
        DateTime? availableAfter = null,
        string? message = TOO_MANY_RETRIES_MESSAGE,
        string? code = TOO_MANY_RETRIES_CODE
    ) => Task.FromResult(TooManyRetries(availableAfter, message, code));

    public static Task<IErrorResult> TooManyRetriesAsyncResult(
        DateTime? availableAfter = null,
        string? message = TOO_MANY_RETRIES_MESSAGE,
        string? code = TOO_MANY_RETRIES_CODE
    ) => Task.FromResult<IErrorResult>(TooManyRetries(availableAfter, message, code));

    public static bool IsTooManyRetriesResult(
        this IResult result,
        [NotNullWhen(true)] out TooManyRetriesResult? tooManyRequestsResult
    )
    {
        if (result is TooManyRetriesResult res)
        {
            tooManyRequestsResult = res;
            return true;
        }

        tooManyRequestsResult = null;
        return false;
    }
}
