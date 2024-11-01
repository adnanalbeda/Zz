namespace Zz.App.Core;

public partial class IdentityRequestHandler<TReq, TRes>
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
                _UserManager?.Dispose();
                _RoleManager?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
#nullable disable
            this._SignInManager = null;
            this._UserManager = null;
            this._RoleManager = null;
#nullable enable
        }
    }
}
