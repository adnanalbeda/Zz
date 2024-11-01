using Microsoft.Extensions.Logging;

namespace Zz.App.Core;

public partial record ProcessLogMessage(
    LogLevel Level,
    Exception? Exception,
    string Message,
    string ProcessId,
    string? ProcessName,
    params object?[] MessageArgs
) : IProcessLogMessage
{
    public DateTime LoggedAt { get; } = DateTime.UtcNow;
}
