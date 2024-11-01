namespace Zz.Api.Controllers;

using System;
using System.Diagnostics;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Zz.App.Core;

[ApiController]
[Route("[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator? mediator;
    protected IMediator _Mediator => mediator ??= GetRequiredService<IMediator>();

    protected async Task<ActionResult<TResult>> HandleAppRequest<TRequest, TResult>(
        TRequest request
    )
        where TRequest : Zz.IRequest<TResult>, IRequest<IRequestResult<TResult>>
    {
        return HandleAppResult(await _Mediator.Send(request, HttpContext.RequestAborted));
    }

    // protected async Task<ActionResult<IEnumerable<TResult>>> HandleRequest<TResult>(
    //     PaginationQuery<TResult> request
    // )
    // {
    //     return HandlePagedResult(await _Mediator.Send(request, HttpContext.RequestAborted));
    // }

    protected ActionResult<T> HandleAppResult<T>(IRequestResult<T> requestResult)
    {
        ArgumentNullException.ThrowIfNull(requestResult);

        var result = requestResult.Result;

        if (result is ISuccessResult<T> sr)
        {
            return sr.Status switch
            {
                Zz.IResult.StatusCode.Success_Ok => Ok(result.Content.Data),
                Zz.IResult.StatusCode.Success_Created => StatusCode(201, result.Content.Data),
                Zz.IResult.StatusCode.Success_Accepted => Accepted(result.Content.Data),
                Zz.IResult.StatusCode.Success_NoContent => NoContent(),
                _ => throw new UnreachableException(
                    "This is not a success result.",
                    new UnreachableException(nameof(result.Status))
                ),
            };
        }

        if (result is Zz.AcceptedResult acceptedResult)
            return Accepted(acceptedResult.Content.Data);

        if (result is IErrorResult errorResult)
            return HandleErrorResult<T>(errorResult);

        throw new UnreachableException(
            "This is not an expected result.",
            new UnreachableException(nameof(result.Status))
        );
    }

    // protected ActionResult<IEnumerable<T>> HandlePagedResult<T>(Result<PagedList<T>> result)
    // {
    //     ArgumentNullException.ThrowIfNull(result);

    //     if (result.Status.IsSuccess())
    //     {
    //         Response.AddPaginationHeader(
    //             result.Value?.CurrentPage ?? 1,
    //             result.Value?.PageSize ?? 0,
    //             result.Value?.TotalCount ?? 0
    //         );
    //         return Ok(result.Value);
    //     }
    //     return HandleErrorResult<IEnumerable<T>>(result);
    // }

    private ActionResult<T> HandleErrorResult<T>(IErrorResult result)
    {
#if DEBUG
        var devMode = GetRequiredService<IHostEnvironment>().IsDevelopment();
#endif
        if (result.Status.IsAborted())
            return StatusCode(499);
        if (result.Status.IsError())
        {
            if (
                result.Status
                is Zz.IResult.StatusCode.Error_InvalidData
                    or Zz.IResult.StatusCode.Error_NotFound
                    or Zz.IResult.StatusCode.Error_UnprocessableRequest
                    or Zz.IResult.StatusCode.Error_WrongContext
            )
                return StatusCode((int)result.Status, result.Content.Data);

            if (
                result.Status
                is Zz.IResult.StatusCode.Error_TooManyRequests
                    or Zz.IResult.StatusCode.Error_TooManyRetries
            )
                return StatusCode((int)result.Status, result.Content.Data);

            if (result is UnexpectedEndResult endResult)
                return StatusCode((int)result.Status, endResult.Content.Data.Message);
        }
        if (result.Status.IsFailure())
        {
#if DEBUG
            if (devMode)
                return StatusCode((int)result.Status, result.Content.Data);
#endif
            return StatusCode(
                500,
                string.Format(
                    "An error occurred while processing your request. Please contact support to further look into this issue. Case reference: [():({0}):{1}]",
                    HttpContext.TraceIdentifier,
                    GetRequiredService<IProcessInfo>().Id
                )
            );
        }

        return StatusCode((int)result.Status, result.Content.Data);
    }

    protected T? GetService<T>()
        where T : class => HttpContext.RequestServices.GetService<T>();

    protected T GetRequiredService<T>()
        where T : class => HttpContext.RequestServices.GetRequiredService<T>();
}
