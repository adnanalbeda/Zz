using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zz.Model.Identity;
using static Zz.DataBase.EFConverter;

namespace Zz.DataBase.Identity;

public class IdentityDataContext<T>
    : IdentityDbContext<User, Role, Id22, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    where T : IdentityDataContext<T>
{
    public IdentityDataContext(DbContextOptions<T> options)
        : base(options) { }

    private DbSet<RefreshToken>? _RefreshTokens;
    public DbSet<RefreshToken> RefreshTokens => _RefreshTokens ??= Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        OnModelCreating_UseIdentity(builder);
    }

    protected static void OnModelCreating_UseIdentity(ModelBuilder builder)
    {
        // ===========
        // User Fields
        builder.Entity<User>(e =>
        {
            e.Metadata.SetSchema("auth");

            // I prefer owns, but not supported with complex types.

            e.HasOne(x => x.MetaData)
                .WithOne(x => x.User)
                .HasPrincipalKey<User>(x => x.Id)
                .HasForeignKey<UserMetaData>(x => x.Id)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Profile)
                .WithOne()
                .HasPrincipalKey<User>(x => x.Id)
                .HasForeignKey<UserProfile>(x => x.Id)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Settings)
                .WithOne()
                .HasPrincipalKey<User>(x => x.Id)
                .HasForeignKey<UserSettings>(x => x.Id)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<UserMetaData>(e =>
        {
            e.Metadata.SetSchema("usr");

            e.HasIndex(x => x.Track_CreatedAt_);
        });
        builder.Entity<UserProfile>(e =>
        {
            e.Metadata.SetSchema("usr");
        });
        builder.Entity<UserSettings>(e =>
        {
            e.Metadata.SetSchema("usr");
        });

        builder.Entity<UserRole>(e =>
        {
            e.Metadata.SetSchema("auth");

            // ==========================
            // Many to Many Relationship
            // When 'User' is removed: Cascade
            e.HasOne(x => x.User)
                .WithMany(y => y.Roles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // When 'Role' is removed: Cascade
            e.HasOne(x => x.Role)
                .WithMany(z => z.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            // X: AppUserRole
            // Y: User
            // Z: Role
            // --------------------------
        });

        // ==========================
        // One to Many Relationship
        builder.Entity<UserLogin>(e =>
        {
            e.Metadata.SetSchema("auth");

            e.HasOne(x => x.User)
                .WithMany(y => y.Logins)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        // When 'User' is removed: Cascade
        // X: AppUserLogin
        // Y: User
        // --------------------------

        // ==========================
        // One to Many Relationship
        builder.Entity<UserClaim>(e =>
        {
            e.Metadata.SetSchema("auth");

            e.HasOne(x => x.User)
                .WithMany(y => y.Claims)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        // When 'User' is removed: Cascade
        // X: UserClaim
        // Y: User
        // --------------------------

        // ==========================
        // One to Many Relationship
        builder.Entity<UserToken>(e =>
        {
            e.Metadata.SetSchema("auth");

            e.HasOne(x => x.User)
                .WithMany(y => y.Tokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        // When 'User' is removed: Cascade
        // X: UserToken
        // Y: User
        // --------------------------

        // ==========================
        // One to Many Relationship
        builder.Entity<RoleClaim>(e =>
        {
            e.Metadata.SetSchema("auth");

            e.HasOne(x => x.Role)
                .WithMany(y => y.Claims)
                .HasForeignKey(x => x.RoleId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });
        // When 'Role' is removed: Cascade
        // X: RoleClaim
        // Y: Role
        // --------------------------

        builder.Entity<RefreshToken>(e =>
        {
            e.Metadata.SetSchema("auth");

            e.HasOne(x => x.User)
                .WithMany(y => y.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .HasPrincipalKey(y => y.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.UseZzCommonConverters();
    }
}
