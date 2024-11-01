namespace Zz.Model;

using System.ComponentModel.DataAnnotations.Schema;

public interface ITrash : IModelType
{
    public TrashData Trashed_ { get; protected set; }

    public void SendToTrash_(string? reason = null, Zz.Identity? by = null)
    {
        Trashed_ = new(UtcNow) { By = by ?? Zz.Identity.Unknown, Reason = reason };
    }
    public void RestoreFromTrash_(string? reason, Zz.Identity? by = null)
    {
        Trashed_ = new(null) { By = by ?? Zz.Identity.Unknown, Reason = reason };
    }
    public void RestoreFromTrash_()
    {
        Trashed_ = TrashData.Available;
    }
}

public interface ITrash<T> : ITrash
    where T : ITrash<T>;

[ComplexType]
public record TrashData(DateTime? Value) : DFlag<DateTime>(Value), IModelType
{
    public Zz.Identity By { get; init; } = Zz.Identity.Unknown;

    private string _reason = string.Empty;
    public string? Reason
    {
        get => _reason;
        init => _reason = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
    }

    private static readonly TrashData _available = new TrashData((DateTime?)null);
    public static TrashData Available => _available;
}
