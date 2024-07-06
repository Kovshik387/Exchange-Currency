using Exchange.Account.Context.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Account.Context.Context;

public partial class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
    : base(options) { }
    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureUser();
        modelBuilder.ConfigureFavorite();
    }

    partial void OnModelCreatingPartial(ModelBuilder mo1delBuilder);
}
