using System.Net;

namespace Zz;

public partial interface IResult
{
    public enum StatusCode
    {
        Success_Ok = AllResultStatusCode._2xx_Ok,
        Success_Created = AllResultStatusCode._2xx_Created,
        Success_NoContent = AllResultStatusCode._2xx_NoContent,
        Success_Accepted = AllResultStatusCode._2xx_Accepted,

        _Unauthenticated = AllResultStatusCode._4xx_Auth_Unauthorized,
        _Unauthorized = AllResultStatusCode._4xx_Auth_Forbidden,

        /// <summary>
        /// When user passed both auth layers and ended up trying to
        /// access/manipulate data they're not supposed to.
        /// </summary>
        Auth_Forbidden = _Unauthorized + 200,
        Auth_PaymentRequired = AllResultStatusCode._4xx_Auth_PaymentRequired,

        Error_RequestAborted = AllResultStatusCode._4xx_Error_Aborted,

        /// <summary>
        /// When request ended up in wrong processor.
        /// </summary>
        Error_WrongContext = AllResultStatusCode._4xx_IAM_A_TEAPOT,
        Error_UnprocessableRequest = AllResultStatusCode._4xx_Error_Unprocessable,

        /// <summary>
        /// Request was processing in correct context but ended without
        /// being not able to figure the appropriate result to return.
        /// </summary>
        Error_UnexpectedEndOfProcess = Error_UnprocessableRequest + 200,

        _BadRequest = AllResultStatusCode._4xx_Error_BadRequest,
        Error_InvalidData = _BadRequest + 200,

        _NotFound = AllResultStatusCode._4xx_Error_NotFound,
        Error_NotFound = _NotFound + 200,

        Error_TooManyRequests = AllResultStatusCode._4xx_Error_TooManyRequests,
        Error_TooManyRetries = Error_TooManyRequests + 200,

        _InternalError = AllResultStatusCode._5xx_Internal,
        Failed_InternalError = _InternalError + 200,

        Failed_NotImplemented = AllResultStatusCode._5xx_NotImplemented,
    }
}

public enum AllResultStatusCode
{
    _2xx_Ok = HttpStatusCode.OK,
    _2xx_Created = HttpStatusCode.Created,
    _2xx_NoContent = HttpStatusCode.NoContent,
    _2xx_Accepted = HttpStatusCode.Accepted,

    _4xx_Auth_Unauthorized = HttpStatusCode.Unauthorized,
    _4xx_Auth_Forbidden = HttpStatusCode.Forbidden,
    _4xx_Auth_PaymentRequired = HttpStatusCode.PaymentRequired,

    _4xx_Error_BadRequest = HttpStatusCode.BadRequest,
    _4xx_Error_NotFound = HttpStatusCode.NotFound,
    _4xx_Error_Unprocessable = HttpStatusCode.UnprocessableEntity,
    _4xx_Error_TooManyRequests = HttpStatusCode.TooManyRequests,

    _4xx_IAM_A_TEAPOT = 418,
    _4xx_Error_Aborted = 499,

    _5xx_Internal = HttpStatusCode.InternalServerError,
    _5xx_NotImplemented = HttpStatusCode.NotImplemented,
    _5xx_Timeout = HttpStatusCode.GatewayTimeout,
    _5xx_NotEnoughStorage = HttpStatusCode.InsufficientStorage,
}
