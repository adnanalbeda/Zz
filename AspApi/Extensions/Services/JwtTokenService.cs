// Beroia Solutions © 2023

namespace Zz.Services.Identity;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Zz.App.Identity;
using Zz.Configs;
using Zz.Model.Identity;

public class JwtTokenService : ITokenService
{
    private readonly JwtIdentityConfigs _config;
    private readonly UserManager<User> _userManager;
    private readonly HttpContext? _httpContext;

    public JwtTokenService(
        JwtIdentityConfigs config,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _config = config;
        _userManager = userManager;
        _httpContext = httpContextAccessor.HttpContext;
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

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.AddClaim(ClaimTypes.Role, role);
        }

        return GetToken(claims, expirationDateTime, origin);
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
            new Claim("sid", sessionId.ToString()),
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.AddClaim(ClaimTypes.Role, role);
        }

        return GetToken(claims, expirationDateTime, origin);
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
        DateTime? expirationDateTime,
        string? audience
    )
    {
        var jwtTokenDescriptor = GetTokenDescriptor(
            claims,
            expirationDateTime ?? DateTime.UtcNow.AddMinutes(30),
            audience
        );

        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var jwtToken = jwtTokenHandler.CreateToken(jwtTokenDescriptor);

        var token = jwtTokenHandler.WriteToken(jwtToken);

        return token;
    }

    private SecurityTokenDescriptor GetTokenDescriptor(
        IEnumerable<Claim> claims,
        DateTime expiration,
        string? audience
    )
    {
        // Signing
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SigningKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        return new()
        {
            IssuedAt = UtcNow,
            NotBefore = UtcNow,
            Expires = expiration,

            Issuer = _config.ValidIssuer!.First(),

            Audience = audience ?? _httpContext?.Request.Headers.Origin,

            Subject = new ClaimsIdentity(claims),

            SigningCredentials = creds,
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
