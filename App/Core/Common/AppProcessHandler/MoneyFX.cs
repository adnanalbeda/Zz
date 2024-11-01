namespace Zz.App.Core;

public partial class AppProcessHandler<T>
{
    private IMoneyFX? _MoneyFX;
    protected IMoneyFX MoneyFX => _MoneyFX ??= GetRequiredService<IMoneyFX>();
}
