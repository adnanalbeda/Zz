namespace Zz.App.Core;

public interface IUserAccessor
{
    public Id22 Id { get; }
    public string? UserName { get; }
    public string? Email { get; }
    public string? DisplayName { get; }
    public IEnumerable<string>? Roles { get; }

    public string? SessionId { get; }

    public bool IsSignedIn() => !Id22.IsEmpty(Id);
    public bool HasRole(string roleName) => Roles is not null && Roles.Contains(roleName);
}
