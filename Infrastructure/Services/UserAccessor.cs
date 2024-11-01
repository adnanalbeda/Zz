using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Zz.App.Core;
using Zz.DataBase;

namespace Zz.Services;

public class UserAccessor(IHttpContextAccessor httpContextAccessor) : IUserAccessor
{
    private HttpContext? _HttpContext => httpContextAccessor.HttpContext;

    private Id22? _id;

    public Id22 Id =>
        _id ??= Id22.Parse(_HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));

    public string? UserName => _HttpContext?.User.FindFirstValue(ClaimTypes.Name);

    public string? Email => _HttpContext?.User.FindFirstValue(ClaimTypes.Email);

    public string? DisplayName => _HttpContext?.User.FindFirstValue(ClaimTypes.GivenName);

    public string? SessionId => _HttpContext?.User.FindFirstValue(ClaimTypes.Sid);

    public IEnumerable<string>? Roles =>
        _HttpContext?.User.FindFirstValue(ClaimTypes.Role)?.Split(' ');
}
