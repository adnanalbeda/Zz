using System;

namespace Zz.Model.Identity;

public class RefreshToken : IModelIdentityType
{
    public RefreshToken(Id22 userId, string token, Id22 id = default)
    {
        Id = Id22.ValueOrNew(id);
        UserId = userId;
        Token = token;
    }

    public Id22 Id { get; private init; }
    public string Token { get; set; }

    public Id22 UserId { get; private init; }
    public User? User { get; set; }

    public DateTime Expires { get; init; } = UtcNow.AddDays(3);
    public bool IsExpired => UtcNow >= Expires;

    public DateTime? RevokedAt { get; set; }
    public bool IsActive => !(IsExpired || RevokedAt.HasValue);
}
