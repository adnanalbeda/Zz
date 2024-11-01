namespace Zz.App.Core;

public partial class NotificationHandler<TNotification>
{
    protected override void Dispose(bool disposing)
    {
        if (!DisposedValue)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                // Anything with dispose.
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
        }
    }
}
