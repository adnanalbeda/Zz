namespace Zz.Configs;

public record IdentityConfigs(
    string? JwtKey,
    string? JwtEncryptionKey,
    IdentityJwtEncryptionByCert? JwtEncryptionCert,
    string? Issuer
);

public record IdentityJwtEncryptionByCert(string PathToPrivate, string PathToPublic);
