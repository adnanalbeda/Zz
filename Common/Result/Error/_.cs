using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Zz;

public class ErrorResult<T> : IErrorResult<T>
    where T : IErrorResultContent
{
    public ErrorResult(IResult.StatusCode statusCode, T content)
    {
        if (!statusCode.IsError())
            throw new UnreachableException("'statusCode' is not an error code.");

        Status = statusCode;
        Content = new(content);
    }

    public IResult.StatusCode Status { get; }

    public ContentWrapper<T> Content { get; }
}

public static partial class Result
{
    public static ErrorResult<T> Error<T>(IResult.StatusCode statusCode, T content)
        where T : IErrorResultContent => new(statusCode, content);

    public static Task<ErrorResult<T>> ErrorAsync<T>(IResult.StatusCode statusCode, T content)
        where T : IErrorResultContent => Task.FromResult(Error(statusCode, content));

    public static bool IsClientFaultResult<T>(
        this IResult result,
        [NotNullWhen(true)] out ErrorResult<T>? clientFaultResult
    )
        where T : IErrorResultContent
    {
        if (result is ErrorResult<T> res)
        {
            clientFaultResult = res;
            return true;
        }

        clientFaultResult = null;
        return false;
    }
}
