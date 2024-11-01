#if DEBUG
namespace Zz.Api.Controllers;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiVersion("1.0")]
public class V1ResultsController : BaseApiVersionedController
{
    public V1ResultsController() { }

    [AllowAnonymous]
    [HttpGet("status-codes")]
    public ActionResult<IResult.StatusCode> GetStatusCodes()
    {
        return Ok(
            NewEnumerable(
                string.Concat(
                    IResult.StatusCode.Success_Ok.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Success_Ok).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Success_Created.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Success_Created).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Success_NoContent.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Success_NoContent).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Success_Accepted.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Success_Accepted).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Auth_Forbidden.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Auth_Forbidden).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Error_InvalidData.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Error_InvalidData).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Error_WrongContext.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Error_WrongContext).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Error_UnprocessableRequest.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Error_UnprocessableRequest).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Error_NotFound.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Error_NotFound).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Error_RequestAborted.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Error_RequestAborted).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Error_TooManyRequests.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Error_TooManyRequests).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Error_TooManyRetries.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Error_TooManyRetries).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Error_UnexpectedEndOfProcess.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Error_UnexpectedEndOfProcess).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Failed_InternalError.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Failed_InternalError).ToString()
                ),
                string.Concat(
                    IResult.StatusCode.Failed_NotImplemented.ToString(),
                    "=",
                    ((int)IResult.StatusCode.Failed_NotImplemented).ToString()
                )
            )
        );
    }

    [AllowAnonymous]
    [HttpGet("accepted")]
    public ActionResult<Zz.AcceptedResult.Data> AcceptedResult()
    {
        return Accepted(Result.Accepted("ID", "URL").Content.Data);
    }

    [AllowAnonymous]
    [HttpGet("no-content")]
    public ActionResult<Null> NoContentResult()
    {
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("not-found")]
    public ActionResult<NotFoundResultContent> GetNotFound()
    {
        return StatusCode((int)IResult.StatusCode.Error_NotFound, Result.NotFound().Content.Data);
    }

    [AllowAnonymous]
    [HttpGet("invalid")]
    public ActionResult<InvalidRequestResultContent> GetInvalid()
    {
        return StatusCode(
            (int)IResult.StatusCode.Error_InvalidData,
            Result
                .InvalidRequest(
                    "INVALID_REQUEST",
                    [
                        new("PROPERTY1", [new("ERR_MSG", "ERR_CODE")]),
                        new("PROPERTY2", [new("ERR_MSG", "ERR_CODE")]),
                    ]
                )
                .Content.Data
        );
    }

    [AllowAnonymous]
    [HttpGet("unprocessable")]
    public ActionResult<UnprocessableRequestResultContent> Unprocessable()
    {
        return UnprocessableEntity(
            Result
                .UnprocessableRequest(
                    "UNPROCESSABLE",
                    "CODE",
                    [new("ERROR_KEY", [new("MSG", "CODE")])]
                )
                .Content.Data
        );
    }

    [AllowAnonymous]
    [HttpGet("unexpected-end")]
    public ActionResult<UnexpectedEndResultContent> UnexpectedEnd()
    {
        var res = Result.UnexpectedEnd();
        return StatusCode((int)res.Status, res.Content.Data);
    }

    [AllowAnonymous]
    [HttpGet("wrong-context")]
    public ActionResult<WrongContextResultContent> WrongContext()
    {
        var res = Result.WrongContext();
        return StatusCode((int)res.Status, res.Content.Data);
    }

    [AllowAnonymous]
    [HttpGet("too-many-requests")]
    public ActionResult<WrongContextResultContent> TooManyRequests()
    {
        var res = Result.TooManyRequests();
        return StatusCode((int)res.Status, res.Content.Data);
    }

    [AllowAnonymous]
    [HttpGet("too-many-retries")]
    public ActionResult<WrongContextResultContent> TooManyRetries()
    {
        var res = Result.TooManyRetries();
        return StatusCode((int)res.Status, res.Content.Data);
    }

    [AllowAnonymous]
    [HttpGet("server-error")]
    public ActionResult GetServerError()
    {
        return StatusCode(
            (int)IResult.StatusCode.Failed_InternalError,
            Result.InternalError("", null!).Content.Data
        );
    }

    [AllowAnonymous]
    [HttpGet("not-implemented")]
    public ActionResult GetNotImplemented()
    {
        return StatusCode(
            (int)IResult.StatusCode.Failed_NotImplemented,
            Result.NotImplemented("ERROR_MSG").Content.Data
        );
    }

    [HttpGet("forbidden")]
    public ActionResult Forbidden()
    {
        return Forbidden();
    }
}
#endif
