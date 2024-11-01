namespace Zz.App.Core;

public partial class RequestPostHandler<TReq, TRes>
{
    private IRequestInfo? requestInfo;
    protected internal IRequestInfo RequestInfo =>
        requestInfo ??= GetRequiredService<IRequestInfo>();
}
