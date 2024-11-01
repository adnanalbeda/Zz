using Microsoft.Extensions.Logging;
using Zz.App;
using Zz.App.Core;

namespace Zz;

public static partial class ProcessLogExtension
{
    public static IProcessLogMessage Debug(
        Exception? Exception,
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        new ProcessLogMessage(
            LogLevel.Debug,
            Exception,
            Message,
            ProcessId,
            ProcessName,
            MessageArgs
        );

    public static IProcessLogMessage Debug(
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    ) => Debug(null, Message, ProcessId, ProcessName, MessageArgs);

    public static IProcessLogMessage Debug<TP>(
        Exception? Exception,
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        Debug(Exception, Message, ProcessId, typeof(TP).FullName, MessageArgs);

    public static IProcessLogMessage Debug<TP>(
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    )
        where TP : IAppProcess => Debug(null, Message, ProcessId, typeof(TP).FullName, MessageArgs);

    public static void Debug(
        this ProcessLogStory logStory,
        Exception? Exception,
        string Message,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        logStory.Register(
            Debug(Exception, Message, logStory.ProcessInfo.Id, ProcessName, MessageArgs)
        );

    public static void Debug(
        this ProcessLogStory logStory,
        string Message,
        string? ProcessName,
        params object?[] MessageArgs
    ) => logStory.Register(Debug(null, Message, logStory.ProcessInfo.Id, ProcessName, MessageArgs));

    public static void Debug<TP>(
        this ProcessLogStory logStory,
        Exception? Exception,
        string Message,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        logStory.Register(
            Debug(Exception, Message, logStory.ProcessInfo.Id, typeof(TP).FullName, MessageArgs)
        );

    public static void Debug<TP>(
        this ProcessLogStory logStory,
        string Message,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        logStory.Register(
            Debug(null, Message, logStory.ProcessInfo.Id, typeof(TP).FullName, MessageArgs)
        );
}
