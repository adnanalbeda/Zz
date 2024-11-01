using Zz.Model;

namespace Zz;

public static class TrackModelExtensions
{
    public static T TrackUpdated<T>(this T track)
        where T : ITrackUpdate<T>
    {
        track.TrackUpdated_();
        return track;
    }

    public static T TrackDoer<T>(this T track, IdentityKind idKind, string value)
        where T : ITrackDoer<T>
    {
        track.TrackDoer_(idKind, value);
        return track;
    }

    public static T TrackChangedBy<T>(this T track, IdentityKind idKind, string value)
        where T : ITrackChangeDoer<T>
    {
        track.TrackChangedBy_(idKind, value);
        return track;
    }

    public static T TrackChangedBy<T>(this T track, Guid userId)
        where T : ITrackChangeDoer<T>
    {
        track.TrackChangedBy_(IdentityKind.Guid, userId.ToString());
        return track;
    }

    public static T AppendCurrentTrackToHistory<T>(this T track)
        where T : ITrackHistory<T>
    {
        track.AppendCurrentTrackToHistory_();
        return track;
    }
}
