using Microsoft.Extensions.Logging;

namespace Zz.App.Core;

public class ProcessLogStory : IProcessLogStory
{
    private readonly List<IProcessLogMessage> _logMessages = new(32);

    protected readonly IProcessInfo processInfo;
    internal IProcessInfo ProcessInfo => processInfo;

    protected readonly IRequestInfo? requestInfo;
    internal IRequestInfo? RequestInfo => requestInfo;

    public IEnumerable<IProcessLogMessage> LogMessages => _logMessages;

    public ProcessLogStory(IProcessInfo processInfo, IRequestInfo? requestInfo)
    {
        this.processInfo = processInfo;
        this.requestInfo = requestInfo;
    }

    public ProcessLogStory(ProcessLogStory baseService)
    {
        this._logMessages = baseService._logMessages;
        this.processInfo = baseService.processInfo;
        this.requestInfo = baseService.requestInfo;
    }

    public void Register(IProcessLogMessage logMessage) => _logMessages.Add(logMessage);
}

public class ProcessLogStory<TP> : ProcessLogStory
    where TP : IAppProcess
{
    private const string BASE_LOG_MESSAGE = "({PLS_TYP} :: {PLS_PID} (:: {PLS_REQ_ID}))";
    private const string BASE_LOG_EX_MESSAGE = BASE_LOG_MESSAGE + " :: ({PLS_EX_MESSAGE})";

    private static string BuildMessage(Exception? exception, string message)
    {
        return string.Concat(
            "===============",
            Environment.NewLine,
            exception is null ? BASE_LOG_MESSAGE : BASE_LOG_EX_MESSAGE,
            Environment.NewLine,
            message,
            Environment.NewLine,
            "***************",
            Environment.NewLine
        );
    }

    private object?[] BuildArgs(Exception? exception, object?[] args)
    {
        return exception is null
            ? args.Prepend(this.RequestInfo?.RequestId)
                .Prepend(this.ProcessInfo.Id)
                .Prepend(typeof(TP).FullName)
                .ToArray()
            : args.Prepend(this.RequestInfo?.RequestId)
                .Prepend(exception.Message)
                .Prepend(this.ProcessInfo.Id)
                .Prepend(typeof(TP).FullName)
                .ToArray();
    }

    private readonly ILogger logger;

    public ProcessLogStory(ILogger<ProcessLogStory<TP>> logger, ProcessLogStory baseService)
        : base(baseService)
    {
        this.logger = logger;
    }

#if DEBUG
    public void LogTrace(Exception? exception, string message, params object?[] args)
    {
        message = BuildMessage(exception, message);
        args = BuildArgs(exception, args);

        this.Trace<TP>(exception, message, args: args);
        logger.LogTrace(exception, message, args: args);
    }

    public void LogTrace(string message, params object?[] args) =>
        LogTrace(null, message, args: args);
#endif

    public void LogDebug(Exception? exception, string message, params object?[] args)
    {
        message = BuildMessage(exception, message);
        args = BuildArgs(exception, args);

        this.Debug<TP>(exception, message, MessageArgs: args);
        logger.LogDebug(exception, message, args: args);
    }

    public void LogDebug(string message, params object?[] args) =>
        LogDebug(null, message, args: args);

    public void LogInformation(Exception? exception, string message, params object?[] args)
    {
        message = BuildMessage(exception, message);
        args = BuildArgs(exception, args);

        this.Information<TP>(exception, message, MessageArgs: args);
        logger.LogInformation(exception, message, args: args);
    }

    public void LogInformation(string message, params object?[] args) =>
        LogInformation(null, message, args: args);

    public void LogWarning(Exception? exception, string message, params object?[] args)
    {
        message = BuildMessage(exception, message);
        args = BuildArgs(exception, args);

        this.Warning<TP>(exception, message, MessageArgs: args);
        logger.LogWarning(exception, message, args: args);
    }

    public void LogWarning(string message, params object?[] args) =>
        LogWarning(null, message, args: args);

    public void LogError(Exception? exception, string message, params object?[] args)
    {
        message = BuildMessage(exception, message);
        args = BuildArgs(exception, args);

        this.Error<TP>(exception, message, MessageArgs: args);
        logger.LogError(exception, message, args: args);
    }

    public void LogError(string message, params object?[] args) =>
        LogError(null, message, args: args);

    public void LogCritical(Exception? exception, string message, params object?[] args)
    {
        message = BuildMessage(exception, message);
        args = BuildArgs(exception, args);

        this.Critical<TP>(exception, message, MessageArgs: args);
        logger.LogCritical(exception, message, args: args);
    }

    public void LogCritical(string message, params object?[] args) =>
        LogCritical(null, message, args: args);
}
