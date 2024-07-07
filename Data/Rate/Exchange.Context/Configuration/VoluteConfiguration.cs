using Exchange.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Context.Configuration
{
    public static class VoluteConfiguration
    {
        public static void ConfigureVolute(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Volute>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("volute_pkey");

                entity.ToTable("volute");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Charcode)
                    .HasMaxLength(50)
                    .HasColumnName("charcode");
                entity.Property(e => e.Idname)
                    .HasMaxLength(255)
                    .HasColumnName("idname");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
                entity.Property(e => e.Nominal).HasColumnName("nominal");
                entity.Property(e => e.Numcode).HasColumnName("numcode");
                entity.Property(e => e.Valcursid).HasColumnName("valcursid");
                entity.Property(e => e.Value).HasColumnName("value");
                entity.Property(e => e.Vunitrate).HasColumnName("vunitrate");

                entity.HasOne(d => d.Valcurs).WithMany(p => p.Volutes)
                    .HasForeignKey(d => d.Valcursid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("volute_valcursid_fkey");
            });
        }
    }
}
