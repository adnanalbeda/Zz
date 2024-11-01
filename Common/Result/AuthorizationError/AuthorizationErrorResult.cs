using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Zz;

public record struct AuthorizationErrorResultContent(
    string Message,
    string Code,
    DateTime? ExpiredAt
) : IErrorResultContent;

public class AuthorizationErrorResult : IErrorResult<AuthorizationErrorResultContent>
{
    public AuthorizationErrorResult(
        IResult.StatusCode statusCode,
        string message,
        string code,
        DateTime? expiredAt
    )
    {
        if (!statusCode.IsAuthorizationError())
            throw new UnreachableException("'statusCode' is not an authorization error code.");

        Status = statusCode;
        Content = new(new(message, code, expiredAt));
    }

    public IResult.StatusCode Status { get; }

    public ContentWrapper<AuthorizationErrorResultContent> Content { get; }
}

public static partial class Result
{
    public static AuthorizationErrorResult AuthorizationError(
        IResult.StatusCode statusCode,
        string message,
        string code,
        DateTime? expiredAt
    ) => new(statusCode, message, code, expiredAt);

    public static Task<AuthorizationErrorResult> AuthorizationErrorAsync(
        IResult.StatusCode statusCode,
        string message,
        string code,
        DateTime? expiredAt
    ) => Task.FromResult(AuthorizationError(statusCode, message, code, expiredAt));

    public static bool IsAuthorizationErrorResult(
        this IResult result,
        [NotNullWhen(true)] out AuthorizationErrorResult? authErrorResult
    )
    {
        if (result is AuthorizationErrorResult res)
        {
            authErrorResult = res;
            return true;
        }

        authErrorResult = null;
        return false;
    }
}
