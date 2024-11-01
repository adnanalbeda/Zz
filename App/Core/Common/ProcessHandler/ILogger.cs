namespace Zz.App.Core;

using System.Diagnostics;
using Microsoft.Extensions.Logging;

public abstract partial class ProcessHandler
{
    private ILogger? logger;
    protected internal ILogger Logger => logger ??= GetRequiredService<ILogger<ProcessHandler>>();

    protected ILogger<TL> GetLogger<TL>() => ServiceProvider.GetLogger<TL>();

    protected void UseLogger<TL>()
    {
#if DEBUG
        this.Logger.LogDebug(
            "Switching process ({PID}) logger to ({TYPE}).",
            ProcessorInfo.Id,
            typeof(TL).FullName
        );
#endif
        logger = GetLogger<TL>();
    }
}
