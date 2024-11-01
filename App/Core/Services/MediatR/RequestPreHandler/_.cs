namespace Zz.App.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

public abstract partial class RequestPreHandler<TReq>(IServiceProvider serviceProvider)
    : AppProcessHandler<TReq>(serviceProvider)
    where TReq : IAppRequest { }
