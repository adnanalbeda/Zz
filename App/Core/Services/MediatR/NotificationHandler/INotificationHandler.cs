namespace Zz.App.Core;

using System.Threading;
using System.Threading.Tasks;
using MediatR;

public partial class NotificationHandler<TNotification> : INotificationHandler<TNotification>
{
    public Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        this.AppProcessLogger.LogInformation("Handling Notification...");
        return Execute(notification, cancellationToken);
    }

    protected abstract Task Execute(
        TNotification notification,
        CancellationToken cancellationToken
    );
}
