using Microsoft.Extensions.Logging;
using Zz.App;
using Zz.App.Core;

namespace Zz;

public static partial class ProcessLogExtension
{
    public static IProcessLogMessage Information(
        Exception? Exception,
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        new ProcessLogMessage(
            LogLevel.Information,
            Exception,
            Message,
            ProcessId,
            ProcessName,
            MessageArgs
        );

    public static IProcessLogMessage Information(
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    ) => Information(null, Message, ProcessId, ProcessName, MessageArgs);

    public static IProcessLogMessage Information<TP>(
        Exception? Exception,
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        Information(Exception, Message, ProcessId, typeof(TP).FullName, MessageArgs);

    public static IProcessLogMessage Information<TP>(
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        Information(null, Message, ProcessId, typeof(TP).FullName, MessageArgs);

    public static void Information(
        this ProcessLogStory logStory,
        Exception? Exception,
        string Message,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        logStory.Register(
            Information(Exception, Message, logStory.ProcessInfo.Id, ProcessName, MessageArgs)
        );

    public static void Information(
        this ProcessLogStory logStory,
        string Message,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        logStory.Register(
            Information(null, Message, logStory.ProcessInfo.Id, ProcessName, MessageArgs)
        );

    public static void Information<TP>(
        this ProcessLogStory logStory,
        Exception? Exception,
        string Message,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        logStory.Register(
            Information(
                Exception,
                Message,
                logStory.ProcessInfo.Id,
                typeof(TP).FullName,
                MessageArgs
            )
        );

    public static void Information<TP>(
        this ProcessLogStory logStory,
        string Message,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        logStory.Register(
            Information(null, Message, logStory.ProcessInfo.Id, typeof(TP).FullName, MessageArgs)
        );
}
