namespace Zz.App.Core;

using System;

public abstract partial class AppProcessHandler<T> : ProcessHandler
    where T : IAppProcess
{
    protected static string RequestName_ => typeof(T).FullName!;

    protected AppProcessHandler(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        UseLogger<T>();
    }
}
