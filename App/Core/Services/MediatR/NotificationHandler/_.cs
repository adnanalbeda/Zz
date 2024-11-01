namespace Zz.App.Core;

using System;

public abstract partial class NotificationHandler<TNotification> : AppProcessHandler<TNotification>
    where TNotification : IAppNotification
{
    protected NotificationHandler(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        UseLogger<TNotification>();
    }
}
