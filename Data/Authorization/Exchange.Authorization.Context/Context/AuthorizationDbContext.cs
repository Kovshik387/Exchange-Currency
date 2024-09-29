using Exchange.Authorization.Context.Configuration;
using Exchange.Authorization.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Authorization.Context.Context;

public class AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : DbContext(options)
{
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ConfigureAccount();
        modelBuilder.ConfigureRefreshToken();
    }
}