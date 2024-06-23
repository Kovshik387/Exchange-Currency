using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Exchange.Context.Migrations.PgSql.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rate_value",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("rate_value_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "volute",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    numcode = table.Column<int>(type: "integer", nullable: false),
                    charcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nominal = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: false),
                    vunitrate = table.Column<decimal>(type: "numeric", nullable: false),
                    valcursid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("volute_pkey", x => x.id);
                    table.ForeignKey(
                        name: "volute_valcursid_fkey",
                        column: x => x.valcursid,
                        principalTable: "rate_value",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "date",
                table: "rate_value",
                column: "date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "rate_value_date_key",
                table: "rate_value",
                column: "date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_volute_valcursid",
                table: "volute",
                column: "valcursid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "volute");

            migrationBuilder.DropTable(
                name: "rate_value");
        }
    }
}
