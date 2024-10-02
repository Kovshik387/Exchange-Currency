using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exchange.Authorization.Context.Migrations.PgSql.Migrations
{
    /// <inheritdoc />
    public partial class InitID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreshes_authorization_IdAccountNavigationId",
                table: "refreshes");

            migrationBuilder.DropIndex(
                name: "IX_refreshes_IdAccountNavigationId",
                table: "refreshes");

            migrationBuilder.DropColumn(
                name: "IdAccountNavigationId",
                table: "refreshes");

            migrationBuilder.RenameColumn(
                name: "Idaccount",
                table: "refreshes",
                newName: "idaccount");

            migrationBuilder.CreateIndex(
                name: "IX_refreshes_idaccount",
                table: "refreshes",
                column: "idaccount");

            migrationBuilder.AddForeignKey(
                name: "refresh_idaccount_fkey",
                table: "refreshes",
                column: "idaccount",
                principalTable: "authorization",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "refresh_idaccount_fkey",
                table: "refreshes");

            migrationBuilder.DropIndex(
                name: "IX_refreshes_idaccount",
                table: "refreshes");

            migrationBuilder.RenameColumn(
                name: "idaccount",
                table: "refreshes",
                newName: "Idaccount");

            migrationBuilder.AddColumn<Guid>(
                name: "IdAccountNavigationId",
                table: "refreshes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_refreshes_IdAccountNavigationId",
                table: "refreshes",
                column: "IdAccountNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_refreshes_authorization_IdAccountNavigationId",
                table: "refreshes",
                column: "IdAccountNavigationId",
                principalTable: "authorization",
                principalColumn: "id");
        }
    }
}
