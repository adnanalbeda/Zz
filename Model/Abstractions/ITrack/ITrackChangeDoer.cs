namespace Zz.Model;

public interface ITrackChangeDoer : ITrackDoer, ITrackChange
{
    public void TrackChangedBy_(IdentityKind idKind, string value)
    {
        this.TrackDoer_(idKind, value);
        this.TrackUpdated_();
    }
}

public interface ITrackChangeDoer<T> : ITrackChangeDoer, ITrackDoer<T>, ITrackChange<T>
    where T : ITrackChangeDoer<T> { }
