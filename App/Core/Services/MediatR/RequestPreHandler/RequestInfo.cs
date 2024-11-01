namespace Zz.App.Core;

public partial class RequestPreHandler<TReq>
{
    private IRequestInfo? requestInfo;
    protected internal IRequestInfo RequestInfo =>
        requestInfo ??= GetRequiredService<IRequestInfo>();
}
