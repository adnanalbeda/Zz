namespace Zz.Configs;

public record JwtIdentityConfigs(
    string? SigningKey,
    string? JwtEncryptionKey,
    IdentityPPPairCert? JwtEncryptionCert,
    IEnumerable<string>? ValidIssuer,
    IEnumerable<string>? ValidAudiences
);

public record IdentityPPPairCert(string PathToPrivate, string PathToPublic);
