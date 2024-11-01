// Beroia Solutions © 2023

namespace Zz.Services.Identity;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Zz.App.Identity;
using Zz.Configs;
using Zz.Model.Identity;

public class TokenService : ITokenService
{
    private readonly IdentityConfigs _config;
    private readonly UserManager<User> _userManager;

    public TokenService(IdentityConfigs config, UserManager<User> userManager)
    {
        _config = config;
        _userManager = userManager;
    }

    public RefreshToken GenerateRefreshToken(Id22 appUserId)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return new RefreshToken(appUserId, Convert.ToBase64String(randomNumber));
    }

    public async Task<string> CreateTokenAsync(
        Id22 userId,
        DateTime? expirationDateTime = null,
        string? origin = null,
        string? agentOrOs = null
    )
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return string.Empty;

        return await CreateTokenAsync(user, expirationDateTime, origin, agentOrOs);
    }

    public async Task<string> CreateTokenAsync(
        User user,
        DateTime? expirationDateTime = null,
        string? origin = null,
        string? agentOrOs = null
    )
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
        };

        claims.AddClaimWhenNotEmpty(ClaimTypes.Email, user.Email);
        claims.AddClaimWhenNotEmpty(ClaimTypes.GivenName, user.Profile.DisplayName);
        claims.AddClaimWhenNotEmpty(ClaimTypes.Webpage, origin);
        claims.AddClaimWhenNotEmpty(ClaimTypes.System, agentOrOs?.GetHashCode().ToString());

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.AddClaim(ClaimTypes.Role, role);
        }

        return GetToken(claims, expirationDateTime ?? DateTime.UtcNow.AddMinutes(60), origin);
    }

    public async Task<string> CreateTokenAsync(
        User user,
        Id22 sessionId,
        DateTime? expirationDateTime = null,
        string? origin = null,
        string? agentOrOs = null
    )
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim("sid", sessionId.ToString()), // sid is not made for this, but that's what I decided it's for.
        };

        // claims.AddClaimWhenNotEmpty(ClaimTypes.Email, user.Email);
        // claims.AddClaimWhenNotEmpty(ClaimTypes.GivenName, user.Profile.DisplayName);
        // claims.AddClaimWhenNotEmpty(ClaimTypes.Webpage, origin);
        // claims.AddClaimWhenNotEmpty(ClaimTypes.System, agentOrOs?.GetHashCode().ToString());

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.AddClaim(ClaimTypes.Role, role);
        }

        return GetToken(claims, expirationDateTime ?? DateTime.UtcNow.AddMinutes(60), origin);
    }

    public Task<string> CreateScopedTokenAsync(
        Id22 id,
        string name,
        string scopeAsRole,
        DateTime expirationDateTime
    )
    {
        var claims = NewEnumerable(
            new Claim(ClaimTypes.NameIdentifier, id.ToShortId()),
            new Claim(ClaimTypes.GivenName, name),
            new Claim(ClaimTypes.Role, scopeAsRole)
        );

        return new Task<string>(() => GetToken(claims, expirationDateTime, null));
    }

    private string GetToken(
        IEnumerable<Claim> claims,
        DateTime expirationDateTime,
        string? audience
    )
    {
        var jwtTokenDescriptor = GetTokenDescriptor(claims, expirationDateTime, audience);

        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var jwtToken = jwtTokenHandler.CreateToken(jwtTokenDescriptor);

        return jwtTokenHandler.WriteToken(jwtToken);
    }

    private SecurityTokenDescriptor GetTokenDescriptor(
        IEnumerable<Claim> claims,
        DateTime expiration,
        string? audience
    )
    {
        // Signing
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.JwtKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // Encrypting
        EncryptingCredentials? encryptionCreds = null;
        if (!string.IsNullOrWhiteSpace(_config.JwtEncryptionCert?.PathToPrivate))
        {
            encryptionCreds = new X509EncryptingCredentials(
                new X509Certificate2(_config.JwtEncryptionCert.PathToPublic)
            );
        }
        else if (!string.IsNullOrWhiteSpace(_config.JwtEncryptionKey))
        {
            var encryptedSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.JwtEncryptionKey!)
            );
            encryptionCreds = new EncryptingCredentials(
                encryptedSecurityKey,
                SecurityAlgorithms.Aes256KW,
                SecurityAlgorithms.Aes256CbcHmacSha512
            );
        }

        return new()
        {
            Subject = new ClaimsIdentity(claims.Append(new Claim("iat", UtcNow.ToIsoString()))),
            Issuer = _config.Issuer,
            Audience = audience,
            NotBefore = UtcNow,
            Expires = expiration,
            SigningCredentials = creds,
            EncryptingCredentials = encryptionCreds,
        };
    }
}

public static class TokensExtensions
{
    public static void AddClaimWhenNotEmpty(
        this ICollection<Claim> claims,
        string claimType,
        string? value
    )
    {
        if (string.IsNullOrWhiteSpace(value))
            return;
        claims.Add(new Claim(claimType, value));
    }

    public static void AddClaim(this ICollection<Claim> claims, string claimType, string value)
    {
        claims.Add(new Claim(claimType, value));
    }
}
