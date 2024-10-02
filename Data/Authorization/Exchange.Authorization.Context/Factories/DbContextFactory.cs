using Exchange.Authorization.Context.Context;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Authorization.Context.Factories;

public class DbContextFactory
{
    private readonly DbContextOptions<AuthorizationDbContext> _options;
    public DbContextFactory(DbContextOptions<AuthorizationDbContext> options)
    {
        _options = options;
    }
    public AuthorizationDbContext Create()
    {
        return new AuthorizationDbContext(_options);
    }
}
