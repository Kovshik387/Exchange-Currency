using Exchange.Context.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
