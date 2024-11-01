namespace Zz.Model;

public interface ITrackUpdate : ITrack
{
    public DateTime Track_UpdatedAt_ { get; protected set; }

    public void TrackUpdated_()
    {
        this.Track_UpdatedAt_ = UtcNow;
    }
}
