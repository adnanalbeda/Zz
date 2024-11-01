namespace Zz;

public interface IRequestBase : IProcess;

public interface IRequestBase<out T> : IRequestBase
    where T : IRequestResult;

public interface IRequest : IRequestBase<IRequestResult>;

public interface IRequest<out TRes, out TData> : IRequestBase<TRes>
    where TRes : IRequestResult<TData>;

public interface IRequest<out TData> : IRequest<IRequestResult<TData>, TData>;
