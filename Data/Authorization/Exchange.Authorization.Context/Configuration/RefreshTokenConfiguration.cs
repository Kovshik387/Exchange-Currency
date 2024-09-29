using Exchange.Authorization.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Authorization.Context.Configuration;

public static class RefreshTokenConfiguration
{
    public static void ConfigureRefreshToken(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_pkey");
            
            entity.ToTable("Refreshes");
            
            entity.Property(e => e.Device)
                .HasMaxLength(255)
                .HasColumnName("device");
            
            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");
            
            entity.Property(e => e.Device)
                .HasMaxLength(255)
                .HasColumnName("device");
            
            //
            // entity.Property(e => e.Expires)
            //     .HasColumnType("expire");
        });
    }
}