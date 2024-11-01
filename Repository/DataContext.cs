using Microsoft.EntityFrameworkCore;
using Zz.DataBase.Identity;

namespace Zz.DataBase;

public partial class DataContext : IdentityDataContext<DataContext>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // enable auto history functionality.
        builder.EnableAutoHistory();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseAllCheckConstraints();
    }
}
