using System.Diagnostics.CodeAnalysis;

namespace Zz;

public record struct InvalidRequestResultContent(
    string Message,
    IEnumerable<InvalidRequestResultContent.Error> Errors
) : IErrorResultContent
{
    public record struct Error(string PropKey, IEnumerable<Error.ErrorDetail> Details)
    {
        public record struct ErrorDetail(string? Message = null, string? Code = null);
    }
}

/// <summary>
/// How is this client fault?<br/>
/// They should send correct data. What they sent is sus af.
/// </summary>
public class InvalidRequestResult : ErrorResult<InvalidRequestResultContent>
{
    public InvalidRequestResult(
        string message,
        IEnumerable<InvalidRequestResultContent.Error> errors
    )
        : base(IResult.StatusCode.Error_InvalidData, new(message, errors)) { }
}

public static partial class Result
{
    public static InvalidRequestResult InvalidRequest(
        string message,
        IEnumerable<InvalidRequestResultContent.Error> errors
    ) => new(message, errors);

    public static Task<InvalidRequestResult> InvalidRequestAsync(
        string message,
        IEnumerable<InvalidRequestResultContent.Error> errors
    ) => Task.FromResult(InvalidRequest(message, errors));

    public static Task<InvalidRequestResult> InvalidRequestAsync(
        string message,
        params InvalidRequestResultContent.Error[] errors
    ) => Task.FromResult(InvalidRequest(message, errors));

    public static Task<IErrorResult> InvalidRequestAsyncResult(
        string message,
        IEnumerable<InvalidRequestResultContent.Error> errors
    ) => Task.FromResult<IErrorResult>(InvalidRequest(message, errors));

    public static Task<IErrorResult> InvalidRequestAsyncResult(
        string message,
        params InvalidRequestResultContent.Error[] errors
    ) => Task.FromResult<IErrorResult>(InvalidRequest(message, errors));

    public static bool IsInvalidRequestResult(
        this IResult result,
        [NotNullWhen(true)] out InvalidRequestResult? invalidRequestResult
    )
    {
        if (result is InvalidRequestResult res)
        {
            invalidRequestResult = res;
            return true;
        }

        invalidRequestResult = null;
        return false;
    }
}
