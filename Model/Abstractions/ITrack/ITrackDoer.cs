namespace Zz.Model;

public interface ITrackDoer : ITrack
{
    public Zz.Identity Track_DoneBy_ { get; protected set; }

    public void TrackDoer_(IdentityKind idKind, string value)
    {
        this.Track_DoneBy_ = new(idKind, value);
    }
}

public interface ITrackDoer<T> : ITrackDoer
    where T : ITrackDoer<T>;
