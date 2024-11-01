using Microsoft.EntityFrameworkCore;
using Zz.Model;

namespace Zz.DataBase;

public partial class MoneyFXDataContext : DbContext
{
    public MoneyFXDataContext(DbContextOptions<MoneyFXDataContext> options)
        : base(options) { }

    private DbSet<Currency>? _Currencies;
    public DbSet<Currency> Currencies => _Currencies ??= Set<Currency>();

    private DbSet<MoneyFXRate>? _MoneyFXRates;
    public DbSet<MoneyFXRate> MoneyFXRates => _MoneyFXRates ??= Set<MoneyFXRate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Currency>(b =>
        {
            b.Metadata.SetSchema("mfx");

            b.HasIndex(x => x.Code).IsUnique(true);

            b.HasIndex(x => x.Track_CreatedAt_);
            b.HasIndex(x => x.Track_UpdatedAt_);

            b.HasMany(x => x.FXRatesHistoryAsSource)
                .WithOne(x => x.SourceCurrency)
                .HasForeignKey(x => x.Source)
                .HasPrincipalKey(x => x.Code)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(x => x.FXRatesHistoryAsTarget)
                .WithOne(x => x.TargetCurrency)
                .HasForeignKey(x => x.Target)
                .HasPrincipalKey(x => x.Code)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MoneyFXRate>(b =>
        {
            b.Metadata.SetSchema("mfx");

            b.HasIndex(x => x.Track_CreatedAt_);

            b.HasIndex(x => x.Source);
            b.HasIndex(x => x.Target);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.UseZzCommonConverters();
    }
}
