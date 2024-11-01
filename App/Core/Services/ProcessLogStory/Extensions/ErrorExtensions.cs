using Microsoft.Extensions.Logging;
using Zz.App;
using Zz.App.Core;

namespace Zz;

public static partial class ProcessLogExtension
{
    public static IProcessLogMessage Error(
        Exception? Exception,
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        new ProcessLogMessage(
            LogLevel.Error,
            Exception,
            Message,
            ProcessId,
            ProcessName,
            MessageArgs
        );

    public static IProcessLogMessage Error(
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    ) => Error(null, Message, ProcessId, ProcessName, MessageArgs);

    public static IProcessLogMessage Error<TP>(
        Exception? Exception,
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        Error(Exception, Message, ProcessId, typeof(TP).FullName, MessageArgs);

    public static IProcessLogMessage Error<TP>(
        string Message,
        string ProcessId,
        string? ProcessName,
        params object?[] MessageArgs
    )
        where TP : IAppProcess => Error(null, Message, ProcessId, typeof(TP).FullName, MessageArgs);

    public static void Error(
        this ProcessLogStory logStory,
        Exception? Exception,
        string Message,
        string? ProcessName,
        params object?[] MessageArgs
    ) =>
        logStory.Register(
            Error(Exception, Message, logStory.ProcessInfo.Id, ProcessName, MessageArgs)
        );

    public static void Error(
        this ProcessLogStory logStory,
        string Message,
        string? ProcessName,
        params object?[] MessageArgs
    ) => logStory.Register(Error(null, Message, logStory.ProcessInfo.Id, ProcessName, MessageArgs));

    public static void Error<TP>(
        this ProcessLogStory logStory,
        Exception? Exception,
        string Message,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        logStory.Register(
            Error(Exception, Message, logStory.ProcessInfo.Id, typeof(TP).FullName, MessageArgs)
        );

    public static void Error<TP>(
        this ProcessLogStory logStory,
        string Message,
        params object?[] MessageArgs
    )
        where TP : IAppProcess =>
        logStory.Register(
            Error(null, Message, logStory.ProcessInfo.Id, typeof(TP).FullName, MessageArgs)
        );
}
