using Microsoft.EntityFrameworkCore;
using Zz.Model.Identity;

namespace Zz.DataBase.Identity;

public class UserSessionsDataContext : DbContext
{
    public UserSessionsDataContext(DbContextOptions<UserSessionsDataContext> options)
        : base(options) { }

    public DbSet<UserSession> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserSession>(e =>
        {
            e.Metadata.SetSchema("usr");

            e.HasKey(x => x.Id);
            e.HasIndex(x => x.UserId);
            e.HasIndex(x => x.Secret).IsUnique(true);
            e.HasIndex(x => x.InvalidatedAt);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.UseZzCommonConverters();
    }
}

// jwt is signed so no need for extra step to get the sid from it.
// sid (stored in LocalStorage and encrypted by secret) will be sent in the header as well.
// if sid is not valid (i.e. not from this secret), invalidate session and remove it.
