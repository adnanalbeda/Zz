using Zz.Model.Identity;

namespace Zz.App.Identity;

public interface ITokenService
{
    public RefreshToken GenerateRefreshToken(Id22 appUserId);

    public Task<string> CreateTokenAsync(
        Id22 userId,
        DateTime? expirationDateTime = null,
        string? origin = null,
        string? agentOrOs = null
    );

    public Task<string> CreateTokenAsync(
        User user,
        DateTime? expirationDateTime = null,
        string? origin = null,
        string? agentOrOs = null
    );

    public Task<string> CreateTokenAsync(
        User user,
        Id22 sessionId,
        DateTime? expirationDateTime = null,
        string? origin = null,
        string? agentOrOs = null
    );

    public Task<string> CreateScopedTokenAsync(
        Id22 id,
        string name,
        string scopeAsRole,
        DateTime expirationDateTime
    );
}
