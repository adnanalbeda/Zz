using Zz.Model;

namespace Zz.Model;

// Use for (List - Create - Update)
public class MoneyFXRateBase(string source, string target, decimal rates)
    : FXRate(source, target, rates)
{
    public long? Id { get; set; }
}

// Use for shape of all necessary data (and List [by Privileged Accounts])
public class MoneyFXRateTable(string source, string target, decimal rates)
    : MoneyFXRateBase(source, target, rates),
        ITrackCreate,
        ITrackDoer,
        ITrash
{
    public new long Id
    {
        get => base.Id ?? default;
        set => base.Id = default == value ? null : value;
    }

    #region ITrack Members

    public DateTime Track_CreatedAt_ { get; init; } = UtcNow;
    public Zz.Identity Track_DoneBy_ { get; set; } = Zz.Identity.Unknown;

    TrashData ITrash.Trashed_ { get; set; } = TrashData.Available;

    #endregion
}

// Use for (Details)
public class MoneyFXRate(string source, string target, decimal rates)
    : MoneyFXRateTable(source, target, rates),
        ITrackDoer<MoneyFXRate>,
        ITrackCreate<MoneyFXRate>
{
    // DX: Relation
    public Currency? SourceCurrency { get; set; }

    // DX: Relation
    public Currency? TargetCurrency { get; set; }
}
