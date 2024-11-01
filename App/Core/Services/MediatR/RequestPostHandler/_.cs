namespace Zz.App.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

public abstract partial class RequestPostHandler<TReq, TRes>(IServiceProvider serviceProvider)
    : AppProcessHandler<TReq>(serviceProvider)
    where TReq : IAppRequest<TRes> { }
