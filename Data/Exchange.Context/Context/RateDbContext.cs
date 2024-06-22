using System;
using System.Collections.Generic;
using Exchange.Context.Configuration;
using Exchange.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Context.Context;

public partial class RateDbContext : DbContext
{
    public RateDbContext(DbContextOptions<RateDbContext> options)
        : base(options) { }

    public virtual DbSet<RateValue> RateValues { get; set; }

    public virtual DbSet<Volute> Volutes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureRateValue();
        modelBuilder.ConfigureVolute();
    }
}
