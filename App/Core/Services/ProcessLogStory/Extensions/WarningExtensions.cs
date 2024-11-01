using Microsoft.Extensions.Logging;
using Zz.App;
using Zz.App.Core;

namespace Zz;

public static partial class ProcessLogExtension
{
    public static IProcessLogMessage Warning(
        Exception? Exception,
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        new ProcessLogMessage(
            LogLevel.Warning,
            Exception,
            Message,
            ProcessId,
            ProcessName,
            MessageArgs
        );

    public static IProcessLogMessage Warning(
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    ) => Warning(null, Message, ProcessId, ProcessName, MessageArgs);

    public static IProcessLogMessage Warning<TP>(
        Exception? Exception,
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        Warning(Exception, Message, ProcessId, typeof(TP).FullName, MessageArgs);

    public static IProcessLogMessage Warning<TP>(
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        Warning(null, Message, ProcessId, typeof(TP).FullName, MessageArgs);

    public static void Warning(
        this ProcessLogStory logStory,
        Exception? Exception,
        string Message,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        logStory.Register(
            Warning(Exception, Message, logStory.ProcessInfo.Id, ProcessName, MessageArgs)
        );

    public static void Warning(
        this ProcessLogStory logStory,
        string Message,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        logStory.Register(
            Warning(null, Message, logStory.ProcessInfo.Id, ProcessName, MessageArgs)
        );

    public static void Warning<TP>(
        this ProcessLogStory logStory,
        Exception? Exception,
        string Message,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        logStory.Register(
            Warning(Exception, Message, logStory.ProcessInfo.Id, typeof(TP).FullName, MessageArgs)
        );

    public static void Warning<TP>(
        this ProcessLogStory logStory,
        string Message,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        logStory.Register(
            Warning(null, Message, logStory.ProcessInfo.Id, typeof(TP).FullName, MessageArgs)
        );
}
