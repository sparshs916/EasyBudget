using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyBudget.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameingOfColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Enrollments",
                newName: "LastSyncedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "BankAccounts",
                newName: "LastSyncedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastSyncedAt",
                table: "Enrollments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "LastSyncedAt",
                table: "BankAccounts",
                newName: "CreatedAt");
        }
    }
}
