using Microsoft.Extensions.Logging;
using Zz.App;
using Zz.App.Core;

namespace Zz;

public static partial class ProcessLogExtension
{
    public static IProcessLogMessage Critical(
        Exception? Exception,
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        new ProcessLogMessage(
            LogLevel.Critical,
            Exception,
            Message,
            ProcessId,
            ProcessName,
            MessageArgs
        );

    public static IProcessLogMessage Critical(
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    ) => Critical(null, Message, ProcessId, ProcessName, MessageArgs);

    public static IProcessLogMessage Critical<TP>(
        Exception? Exception,
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        Critical(Exception, Message, ProcessId, typeof(TP).FullName, MessageArgs);

    public static IProcessLogMessage Critical<TP>(
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        Critical(null, Message, ProcessId, typeof(TP).FullName, MessageArgs);

    public static void Critical(
        this ProcessLogStory logStory,
        Exception? Exception,
        string Message,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        logStory.Register(
            Critical(Exception, Message, logStory.ProcessInfo.Id, ProcessName, MessageArgs)
        );

    public static void Critical(
        this ProcessLogStory logStory,
        string Message,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        logStory.Register(
            Critical(null, Message, logStory.ProcessInfo.Id, ProcessName, MessageArgs)
        );

    public static void Critical<TP>(
        this ProcessLogStory logStory,
        Exception? Exception,
        string Message,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        logStory.Register(
            Critical(Exception, Message, logStory.ProcessInfo.Id, typeof(TP).FullName, MessageArgs)
        );

    public static void Critical<TP>(
        this ProcessLogStory logStory,
        string Message,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        logStory.Register(
            Critical(null, Message, logStory.ProcessInfo.Id, typeof(TP).FullName, MessageArgs)
        );
}
