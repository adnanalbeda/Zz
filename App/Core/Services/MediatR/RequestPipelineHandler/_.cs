namespace Zz.App.Core;

using System;

public abstract partial class RequestPipelineHandler<TReq, TRes>(IServiceProvider serviceProvider)
    : AppProcessHandler<TReq>(serviceProvider)
    where TReq : IAppRequest<TRes> { }
