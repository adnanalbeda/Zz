using System.Security.Cryptography;
using System.Text;

namespace Zz.Model.Identity;

public class UserSessionBase(Id22 userId, Id22 id = default) : IModelIdentityType
{
    public Id22 Id { get; set; } = Id22.ValueOrNew(id);
    public Id22 UserId { get; set; } = userId;

    public DateTime StartedAt { get; private init; } = UtcNow;

    public string? Name { get; set; }
    public string? DeviceId { get; set; }
}

public sealed class UserSession : UserSessionBase
{
    public UserSession(Id22 userId, Id22 id = default)
        : base(userId, id)
    {
        this.Secret = UtcNow.AddSeconds(-1 * NextRandom()).ToSecureRandomId();
    }

    public UserSession(Id22 userId, Id22 id, string secret)
        : base(userId, id)
    {
        this.Secret = secret;
    }

    public string Secret { get; private init; }

    public DateTime? InvalidatedAt { get; private set; }

    public void EndSession()
    {
        if (InvalidatedAt.HasValue)
            return;
        InvalidatedAt = UtcNow;
    }

    public bool ValidateSession(string encryptedSid)
    {
        if (InvalidatedAt.HasValue)
            return false;
        if (encryptedSid == GetEncryptedSessionId())
            return true;
        InvalidatedAt = UtcNow;
        return false;
    }

    public string GetEncryptedSessionId() => ComputeSignature();

    private string ComputeSessionIdHash()
    {
        using var sha = SHA512.Create();
        byte[] hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(Id.ToShortId()));
        return Convert.ToBase64String(hashedBytes);
    }

    private string ComputeSignature()
    {
        using var hmacsha256 = new HMACSHA256(Convert.FromBase64String(this.Secret));
        var bytes = Encoding.UTF8.GetBytes(ComputeSessionIdHash());
        var hashedBytes = hmacsha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashedBytes);
    }
}

// jwt is signed so no need for extra step to get the sid from it.
// sid (stored in LocalStorage and encrypted by secret) will be sent in the header as well.
// if sid is not valid (i.e. not from this secret), invalidate session and remove it.
