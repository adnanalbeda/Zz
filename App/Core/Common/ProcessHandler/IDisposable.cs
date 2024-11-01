namespace Zz.App.Core;

public abstract partial class ProcessHandler : IDisposable
{
    private bool disposedValue;
    protected bool DisposedValue => disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!DisposedValue)
        {
            disposedValue = true;

            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                // Anything with dispose.
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
#nullable disable
            serviceProvider = null;
            processorInfo = null;
            logger = null;
#nullable enable
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
