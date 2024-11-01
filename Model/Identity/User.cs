using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zz;

namespace Zz.Model.Identity;

public class User : IdentityUser<Id22>, IModelIdentityType
{
    public User(Id22 id, string userName)
        : base(userName)
    {
        this.Id = Id22.ValueOrNew(id);
    }

    public IEnumerable<UserRole> Roles { get; set; } = [];
    public IEnumerable<UserLogin> Logins { get; set; } = [];
    public IEnumerable<UserClaim> Claims { get; set; } = [];
    public IEnumerable<UserToken> Tokens { get; set; } = [];
    public IEnumerable<RefreshToken> RefreshTokens { get; set; } = [];

    public required UserMetaData MetaData { get; set; } = new(default);
    public required UserProfile Profile { get; set; } = new(default);
    public required UserSettings Settings { get; set; } = new(default);
}

public record UserMetaData(Id22 Id) : ITrash<UserMetaData>, ITrackChange<UserMetaData>
{
    public DateTime Track_CreatedAt_ { get; private init; } = UtcNow;
    public DateTime Track_UpdatedAt_ { get; set; } = UtcNow;

    public TrashData Trashed_ { get; set; } = TrashData.Available;

    public User? User { get; set; }
}

// Everything here should be nullable
public record UserProfile(Id22 Id) : IModelType
{
    public string DisplayName { get; set; } = string.Empty;

    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }

    public Gender Gender { get; set; } = Gender._;

    public User? User { get; set; }
}

// Everything here should have a default value
public record UserSettings(Id22 Id) : IModelType
{
    public ColorScheme ColorSchemePreference { get; set; } = ColorScheme.System;

    public User? User { get; set; }
}
