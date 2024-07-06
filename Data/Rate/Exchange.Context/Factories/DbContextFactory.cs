using Exchange.Context.Context;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Context.Factories;

public class DbContextFactory
{
    private readonly DbContextOptions<RateDbContext> options;

    public DbContextFactory(DbContextOptions<RateDbContext> options)
    {
        this.options = options;
    }

    public RateDbContext Create()
    {
        return new RateDbContext(options);
    }
}
