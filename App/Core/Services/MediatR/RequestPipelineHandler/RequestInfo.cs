namespace Zz.App.Core;

public partial class RequestPipelineHandler<TReq, TRes>
{
    private IRequestInfo? requestInfo;
    protected internal IRequestInfo RequestInfo =>
        requestInfo ??= GetRequiredService<IRequestInfo>();
}
