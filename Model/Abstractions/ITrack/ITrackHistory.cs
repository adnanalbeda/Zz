namespace Zz.Model;

using System.Text.Json;
using System.Text.Json.Serialization;

public interface ITrackHistory<T> : ITrackChangeDoer<T>
    where T : ITrackHistory<T>
{
    [JsonIgnore]
    public IEnumerable<HistoryEntry> Track_History_ { get; protected set; }

    public void AppendCurrentTrackToHistory_()
    {
        Track_History_ = Track_History_.Append(new(UtcNow, JsonSerializer.Serialize((T)this)));
    }

    public record HistoryEntry(DateTime DateTime, string Value);
}
