namespace Zz;

public static class ResultExtensions
{
    public static bool IsSuccess(this IResult.StatusCode statusCode)
    {
        switch (statusCode)
        {
            case IResult.StatusCode.Success_Ok:
            case IResult.StatusCode.Success_Created:
            case IResult.StatusCode.Success_NoContent:
            case IResult.StatusCode.Success_Accepted:
                return true;
            default:
                return false;
        }
    }

    public static bool IsAuthorizationError(this IResult.StatusCode statusCode)
    {
        switch (statusCode)
        {
            case IResult.StatusCode._Unauthenticated:
            case IResult.StatusCode._Unauthorized:
            case IResult.StatusCode.Auth_Forbidden:
            case IResult.StatusCode.Auth_PaymentRequired:
                return true;
            default:
                return false;
        }
    }

    public static bool IsError(this IResult.StatusCode statusCode)
    {
        switch (statusCode)
        {
            case IResult.StatusCode.Error_RequestAborted:
            case IResult.StatusCode.Error_InvalidData:
            case IResult.StatusCode.Error_NotFound:
            case IResult.StatusCode.Error_TooManyRequests:
            case IResult.StatusCode.Error_TooManyRetries:
            case IResult.StatusCode.Error_UnexpectedEndOfProcess:
            case IResult.StatusCode.Error_UnprocessableRequest:
            case IResult.StatusCode.Error_WrongContext:
                return true;
            default:
                return false;
        }
    }

    public static bool IsFailure(this IResult.StatusCode statusCode)
    {
        switch (statusCode)
        {
            case IResult.StatusCode.Failed_InternalError:
            case IResult.StatusCode.Failed_NotImplemented:
                return true;
            default:
                return false;
        }
    }

    public static bool IsAborted(this IResult.StatusCode statusCode) =>
        statusCode == IResult.StatusCode.Error_RequestAborted;
}
