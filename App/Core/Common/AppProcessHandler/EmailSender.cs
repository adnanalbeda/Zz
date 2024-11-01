namespace Zz.App.Core;

public partial class AppProcessHandler<T>
{
    private IEmailSender? _EmailSender;
    protected IEmailSender EmailSender => _EmailSender ??= GetRequiredService<IEmailSender>();
}
