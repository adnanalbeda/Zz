namespace Zz.App.Core;

using System.Collections;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;

public interface IProcessLogMessage
{
    public DateTime LoggedAt { get; }
    public string ProcessId { get; }
    public string? ProcessName { get; }
    public LogLevel Level { get; }
    public Exception? Exception { get; }
    public string Message { get; }
    public object?[] MessageArgs { get; }

#if DEBUG
    public void InternalLog()
    {
        string internalMessage = string.Concat(
            "Log Message: {0}",
            Environment.NewLine,
            "Message Args: {1}"
        );
        string msgArgs = JsonSerializer.Serialize(MessageArgs);
        if (Level is LogLevel.Trace or LogLevel.Debug or LogLevel.Information)
        {
            Trace.TraceInformation(internalMessage, Message, msgArgs);
            Console.WriteLine(internalMessage, Message, msgArgs);
        }
        if (Level is LogLevel.Warning)
        {
            Trace.TraceWarning(Message, msgArgs);
            Console.WriteLine(Message, msgArgs);
        }
        if (Level is LogLevel.Error or LogLevel.Critical)
        {
            Trace.TraceError(Message, msgArgs);
            Console.Beep();
            Console.Error.WriteLine(Message, msgArgs);
        }
    }
#endif

    public void Log(ILogger logger)
    {
        logger.Log(Level, Exception, Message, MessageArgs);
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}

public interface IProcessLogStory : IEnumerable<IProcessLogMessage>
{
    public IEnumerable<IProcessLogMessage> LogMessages { get; }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this.LogMessages);
    }

    IEnumerator<IProcessLogMessage> IEnumerable<IProcessLogMessage>.GetEnumerator() =>
        LogMessages.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => LogMessages.GetEnumerator();
}
