using System.Diagnostics.CodeAnalysis;

namespace Zz;

public record struct UnprocessableRequestResultContent(
    string Message,
    string? Code = null,
    IEnumerable<InvalidRequestResultContent.Error>? Errors = null
) : IErrorResultContent;

/// <summary>
/// How is this client fault?<br/>
/// They shouldn't send data which look right.
/// They should send data which are right.
/// </summary>
public class UnprocessableRequestResult(
    string message,
    string? code = null,
    IEnumerable<InvalidRequestResultContent.Error>? errors = null
)
    : ErrorResult<UnprocessableRequestResultContent>(
        IResult.StatusCode.Error_UnprocessableRequest,
        new(message, code, errors)
    );

public static partial class Result
{
    public static UnprocessableRequestResult UnprocessableRequest(
        string message,
        string? code = null,
        IEnumerable<InvalidRequestResultContent.Error>? errors = null
    ) => new(message, code, errors);

    public static Task<UnprocessableRequestResult> UnprocessableRequestAsync(
        string message,
        string? code = null,
        IEnumerable<InvalidRequestResultContent.Error>? errors = null
    ) => Task.FromResult(UnprocessableRequest(message, code, errors));

    public static Task<IErrorResult> UnprocessableRequestAsyncResult(
        string message,
        string? code = null,
        IEnumerable<InvalidRequestResultContent.Error>? errors = null
    ) => Task.FromResult((IErrorResult)UnprocessableRequest(message, code, errors));

    public static bool IsUnprocessableRequestResult(
        this IResult result,
        [NotNullWhen(true)] out UnprocessableRequestResult? unprocessableRequestResult
    )
    {
        if (result is UnprocessableRequestResult res)
        {
            unprocessableRequestResult = res;
            return true;
        }

        unprocessableRequestResult = null;
        return false;
    }
}
