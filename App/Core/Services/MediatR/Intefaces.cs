namespace Zz.App;

public interface IAppNotification : IAppProcess, MediatR.INotification
{
    public string NotificationTaskId { get; }
}

public interface IAppRequestBase : IAppProcess, IRequestBase<IRequestResult>, MediatR.IBaseRequest;

public interface IAppRequest : IAppRequestBase, IRequest, MediatR.IRequest<IRequestResult>;

public interface IAppRequest<T> : IAppRequest, IRequest<T>, MediatR.IRequest<IRequestResult<T>>;

public interface IQuery : IAppRequest;

public interface IQuery<T> : IQuery, IAppRequest<T>;

public interface IMayValidate;

public interface ICommand : IAppRequest, IMayValidate;

public interface ICommand<T> : ICommand, IAppRequest<T>;

public interface IAppRequestHandler<TReq, TRes>
    : MediatR.IRequestHandler<TReq, IRequestResult<TRes>>
    where TReq : IAppRequest<TRes>;

public interface IAppNotificationHandler<in TNotification>
    : MediatR.INotificationHandler<TNotification>
    where TNotification : IAppNotification;

public interface IAppPreRequestHandler<in TReq> : MediatR.Pipeline.IRequestPreProcessor<TReq>
    where TReq : IAppRequest;

public interface IAppPostRequestHandler<in TReq, TRes>
    : MediatR.Pipeline.IRequestPostProcessor<TReq, IRequestResult<TRes>>
    where TReq : IAppRequest<TRes>;

public interface IAppRequestPipelineBehavior<in TReq, TReqRes, TRes>
    where TReq : IAppRequest<TRes>
    where TReqRes : IRequestResult<TRes>;

public interface IAppRequestPipelineBehavior<in TReq, TRes>
    : IAppRequestPipelineBehavior<TReq, IRequestResult<TRes>, TRes>,
        MediatR.IPipelineBehavior<TReq, IRequestResult<TRes>>
    where TReq : IAppRequest<TRes>;
