namespace Zz.Configs;

public record SmtpEmailConfigs(
    string? SenderName,
    string? Email,
    string? Password,
    string? SmtpUrl,
    int? Port,
    SecurityProtocolType? SecurityProtocolType
);

public enum SecurityProtocolType
{
    Default = 0,
    SSL,
    TLS,
    TLS_IfAvailable,
    _ = Default,
}
