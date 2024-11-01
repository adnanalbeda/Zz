namespace Zz.Model;

public interface ITrackChange : ITrackCreate, ITrackUpdate;

public interface ITrackCreate<T> : ITrackCreate
    where T : ITrackCreate<T>;

public interface ITrackUpdate<T> : ITrackUpdate
    where T : ITrackUpdate<T>;

public interface ITrackChange<T> : ITrackChange, ITrackCreate<T>, ITrackUpdate<T>
    where T : ITrackChange<T>;
