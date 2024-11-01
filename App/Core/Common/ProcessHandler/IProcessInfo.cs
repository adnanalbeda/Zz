namespace Zz.App.Core;

public partial class ProcessHandler
{
    private IProcessInfo? processorInfo;
    protected internal IProcessInfo ProcessorInfo =>
        processorInfo ??= GetRequiredService<IProcessInfo>();
}

public interface IProcessInfo
{
    public string Id { get; }

    public DateTime ReceivedAt { get; }
    public DateTime? StartedAt { get; protected set; }
    public DateTime? EndedAt { get; protected set; }

    public void Started()
    {
        this.StartedAt ??= DateTime.UtcNow;
    }
    public void Ended()
    {
        this.EndedAt ??= DateTime.UtcNow;
    }
}
