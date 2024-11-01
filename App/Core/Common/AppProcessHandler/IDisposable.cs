namespace Zz.App.Core;

public abstract partial class AppProcessHandler<T>
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
                _DataContext?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
#nullable disable
            _Mapper = null;
            _DataContext = null;
            _EmailSender = null;
            _UserAccessor = null;
            _Mediator = null;
            _MoneyFX = null;
            _AppProcessLogger = null;
#nullable enable
        }
    }
}
