namespace Zz.Services;

using Microsoft.AspNetCore.Http;
using Zz.App.Core;

public class RequestInfo : ProcessInfo, IRequestInfo
{
    public RequestInfo(
        IHttpContextAccessor httpContextAccessor,
        IProcessInfo processInfo,
        ILogger<RequestInfo> logger
    )
        : base(processInfo)
    {
        HttpContext = httpContextAccessor.HttpContext!;
        logger.LogInformation("({REQ_ID}) is attached to ({TID})", this.RequestId, this.Id);
    }

    public HttpContext HttpContext { get; }

    public string? Origin => HttpContext.Request.Headers.Origin;

    public string? Path => HttpContext.Request.Path;

    public string? Method => HttpContext.Request.Method;

    public string RequestId => HttpContext.TraceIdentifier;

    public string? Protocol => HttpContext.Request.Protocol;

    public string? Action => string.Empty;

    public string? Parameters => string.Empty;
}
