using Microsoft.AspNetCore.Http;

namespace Zz.App.Core;

public partial class RequestHandler<TReq, TRes>
{
    private IRequestInfo? requestInfo;
    protected internal IRequestInfo RequestInfo =>
        requestInfo ??= GetRequiredService<IRequestInfo>();
}

public interface IRequestInfo : IProcessInfo
{
    public HttpContext HttpContext { get; }
    public string RequestId { get; }

    /// <summary>
    /// (e.g HTTP / WS)
    /// </summary>
    public string? Protocol { get; }

    public string? Method { get; }
    public string? Action { get; }

    public string? Origin { get; }
    public string? Path { get; }
    public string? Parameters { get; }
}
