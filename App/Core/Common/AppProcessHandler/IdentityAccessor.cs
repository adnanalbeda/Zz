namespace Zz.App.Core;

public partial class AppProcessHandler<T>
{
    private IUserAccessor? _UserAccessor;
    protected IUserAccessor User => _UserAccessor ??= GetRequiredService<IUserAccessor>();
}
