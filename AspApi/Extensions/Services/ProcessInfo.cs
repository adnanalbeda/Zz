namespace Zz.Services;

using System;
using Microsoft.AspNetCore.Http;
using Zz.App.Core;

public class ProcessInfo : IProcessInfo
{
    public ProcessInfo() { }

    protected ProcessInfo(IProcessInfo processInfo)
    {
        this.Id = processInfo.Id;
        this.ReceivedAt = processInfo.ReceivedAt;
        StartedAt = processInfo.StartedAt;
        EndedAt = processInfo.EndedAt;
    }

    public string Id { get; } =
        DateTime.UtcNow.ToShortId(DateTimeHelpers.ShortIdEndWith.Millisecond);

    public DateTime ReceivedAt { get; } = DateTime.UtcNow;

    public DateTime? StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }

    DateTime? IProcessInfo.StartedAt
    {
        get => this.StartedAt;
        set => this.StartedAt = value;
    }
    DateTime? IProcessInfo.EndedAt
    {
        get => this.EndedAt;
        set => this.EndedAt = value;
    }
}
