using Zz.Configs;

namespace Zz;

public class ZzAppConfigsResolver(IConfiguration configuration)
{
    public const string AppSettingsKey = "ZzAppConfigs";

    public IConfiguration Configuration { get; } = configuration;

    public SmtpEmailConfigs? SmtpEmailConfigs =>
        Configuration.GetSection($"{AppSettingsKey}:SmtpEmailConfigs").Get<SmtpEmailConfigs>();

    public JwtIdentityConfigs? JwtIdentityConfigs =>
        Configuration.GetSection($"Authentication:Schemes:Bearer").Get<JwtIdentityConfigs>();
}
