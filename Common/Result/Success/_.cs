using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Zz;

public record struct SuccessResultContent<T>(T Value) : ISuccessResultContent<T>;

public class SuccessResult<T> : ISuccessResult<T, SuccessResultContent<T>>
{
    public SuccessResult(IResult.StatusCode statusCode, T value)
    {
        if (!statusCode.IsSuccess())
            throw new UnreachableException("'statusCode' is not a success code.");

        Status = statusCode;
        Content = new(new(value));
    }

    public IResult.StatusCode Status { get; }

    public ContentWrapper<SuccessResultContent<T>> Content { get; }
}

public static partial class Result
{
    public static SuccessResult<T> Success<T>(IResult.StatusCode statusCode, T value) =>
        new(statusCode, value);

    public static Task<SuccessResult<T>> SuccessAsync<T>(IResult.StatusCode statusCode, T value) =>
        Task.FromResult(Success(statusCode, value));

    public static bool IsSuccessResult<T>(
        this IResult result,
        [NotNullWhen(true)] out SuccessResult<T>? successResult
    )
    {
        if (result is SuccessResult<T> res)
        {
            successResult = res;
            return true;
        }

        successResult = null;
        return false;
    }
}
