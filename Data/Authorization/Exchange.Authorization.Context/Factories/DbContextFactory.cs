using Exchange.Authorization.Context.Context;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Authorization.Context.Factories;

public class DbContextFactory(DbContextOptions<AuthorizationDbContext> options)
{
    public AuthorizationDbContext Create()
    {
        return new AuthorizationDbContext(options);
    }
}
