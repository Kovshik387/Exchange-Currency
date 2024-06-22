using Exchange.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Context.Configuration
{
    public static class RateValueConfiguration
    {
        public static void ConfigureRateValue(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RateValue>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("rate_value_pkey");

                entity.ToTable("rate_value");

                entity.HasIndex(e => e.Date, "date").IsUnique();

                entity.HasIndex(e => e.Date, "rate_value_date_key").IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });
        }
    }
}
