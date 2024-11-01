using Zz.Configs;

namespace Zz;

public class ZzAppConfigs
{
    public const string AppSettingsKey = "ZzAppConfigs";

    public ZzAppConfigs(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public SmtpEmailConfigs? SmtpEmailConfigs =>
        Configuration.GetSection($"{AppSettingsKey}:SmtpEmailConfigs").Get<SmtpEmailConfigs>();

    public IdentityConfigs? IdentityConfigs =>
        Configuration.GetSection($"{AppSettingsKey}:IdentityConfigs").Get<IdentityConfigs>();
}
