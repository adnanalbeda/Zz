using Microsoft.Extensions.Logging;
using Zz.App;
using Zz.App.Core;

namespace Zz;

public static partial class ProcessLogExtension
{
    public static IProcessLogMessage Trace(
        Exception? exception,
        string message,
        string processId,
        string? processName,
        params object?[] args
    ) => new ProcessLogMessage(LogLevel.Trace, exception, message, processId, processName, args);

    public static IProcessLogMessage Trace(
        string message,
        string processId,
        string? processName,
        params object?[] args
    ) => Trace(null, message, processId, processName, args);

    public static IProcessLogMessage Trace<TP>(
        Exception? exception,
        string message,
        string processId,
        params object?[] args
    )
        where TP : IAppProcess => Trace(exception, message, processId, typeof(TP).FullName, args);

    public static IProcessLogMessage Trace<TP>(
        string message,
        string processId,
        params object?[] args
    )
        where TP : IAppProcess => Trace(null, message, processId, typeof(TP).FullName, args);

    public static void Trace(
        this ProcessLogStory logStory,
        Exception? exception,
        string message,
        string? processName,
        params object?[] args
    ) => logStory.Register(Trace(exception, message, logStory.ProcessInfo.Id, processName, args));

    public static void Trace(
        this ProcessLogStory logStory,
        string message,
        string? processName,
        params object?[] args
    ) => logStory.Register(Trace(null, message, logStory.ProcessInfo.Id, processName, args));

    public static void Trace<TP>(
        this ProcessLogStory logStory,
        Exception? exception,
        string message,
        params object?[] args
    )
        where TP : IAppProcess =>
        logStory.Register(
            Trace(exception, message, logStory.ProcessInfo.Id, typeof(TP).FullName, args)
        );

    public static void Trace<TP>(
        this ProcessLogStory logStory,
        string message,
        params object?[] args
    )
        where TP : IAppProcess =>
        logStory.Register(Trace(null, message, logStory.ProcessInfo.Id, typeof(TP).FullName, args));
}
