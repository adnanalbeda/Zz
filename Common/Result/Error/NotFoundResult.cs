using System.Diagnostics.CodeAnalysis;

namespace Zz;

public record struct NotFoundResultContent(string Message, string Code) : IErrorResultContent;

/// <summary>
/// How is this client fault? They shouldn't request data that don't exist.
/// </summary>
public class NotFoundResult(string message, string code)
    : ErrorResult<NotFoundResultContent>(IResult.StatusCode.Error_NotFound, new(message, code));

public static partial class Result
{
    public const string NOT_FOUND_STRING = "NOT_FOUND";

    public static NotFoundResult NotFound(
        string message = NOT_FOUND_STRING,
        string code = NOT_FOUND_STRING
    ) => new(message, code);

    public static Task<NotFoundResult> NotFoundAsync(
        string message = NOT_FOUND_STRING,
        string code = NOT_FOUND_STRING
    ) => Task.FromResult(NotFound(message, code));

    public static Task<IErrorResult> NotFoundAsyncResult(
        string message = NOT_FOUND_STRING,
        string code = NOT_FOUND_STRING
    ) => Task.FromResult<IErrorResult>(NotFound(message, code));

    public static bool IsNotFoundResult(
        this IResult result,
        [NotNullWhen(true)] out NotFoundResult? notFoundResult
    )
    {
        if (result is NotFoundResult res)
        {
            notFoundResult = res;
            return true;
        }

        notFoundResult = null;
        return false;
    }
}
