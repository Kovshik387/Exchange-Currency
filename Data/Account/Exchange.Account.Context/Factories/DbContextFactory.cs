using Exchange.Account.Context.Context;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Account.Context.Factories;

public class DbContextFactory
{
    private readonly DbContextOptions<AccountDbContext> options;

    public DbContextFactory(DbContextOptions<AccountDbContext> options)
    {
        this.options = options;
    }

    public AccountDbContext Create()
    {
        return new AccountDbContext(options);
    }
}
