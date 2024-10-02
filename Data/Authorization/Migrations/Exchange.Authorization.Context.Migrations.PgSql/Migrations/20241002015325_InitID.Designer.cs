﻿// <auto-generated />
using System;
using Exchange.Authorization.Context.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Exchange.Authorization.Context.Migrations.PgSql.Migrations
{
    [DbContext(typeof(AuthorizationDbContext))]
    [Migration("20241002015325_InitID")]
    partial class InitID
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Exchange.Authorization.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("EmailNormalized")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password_hash");

                    b.HasKey("Id")
                        .HasName("auth_pkey");

                    b.HasIndex(new[] { "Email" }, "auth_email_key")
                        .IsUnique();

                    b.HasIndex(new[] { "EmailNormalized" }, "auth_normalized_email_key")
                        .IsUnique();

                    b.ToTable("authorization", (string)null);
                });

            modelBuilder.Entity("Exchange.Authorization.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Device")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("device");

                    b.Property<Guid?>("Idaccount")
                        .HasColumnType("uuid")
                        .HasColumnName("idaccount");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.HasKey("Id")
                        .HasName("refresh_pkey");

                    b.HasIndex("Idaccount");

                    b.ToTable("refreshes", (string)null);
                });

            modelBuilder.Entity("Exchange.Authorization.Entities.RefreshToken", b =>
                {
                    b.HasOne("Exchange.Authorization.Entities.Account", "IdAccountNavigation")
                        .WithMany("Refreshes")
                        .HasForeignKey("Idaccount")
                        .HasConstraintName("refresh_idaccount_fkey");

                    b.Navigation("IdAccountNavigation");
                });

            modelBuilder.Entity("Exchange.Authorization.Entities.Account", b =>
                {
                    b.Navigation("Refreshes");
                });
#pragma warning restore 612, 618
        }
    }
}
