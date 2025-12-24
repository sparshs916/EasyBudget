using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyBudget.Api.Migrations
{
    /// <inheritdoc />
    public partial class Hopefully_I_Have_Finalized_The_Models_Now : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_BankAccounts_BankAccountGuid",
                table: "Balances");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Enrollments_EnrollmentId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Users_UserId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_BankAccountGuid",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BankAccountGuid",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_EnrollmentId",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Balances_BankAccountGuid",
                table: "Balances");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "BankAccountGuid",
                table: "Balances");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "BankAccountGuid",
                table: "Transactions",
                newName: "ProcessingStatus");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Enrollments",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "EncryptedAccessToken",
                table: "Enrollments",
                newName: "AccessToken");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:hstore", ",,");

            // Use raw SQL for type conversions that PostgreSQL can't do automatically
            migrationBuilder.Sql("ALTER TABLE \"Transactions\" ALTER COLUMN \"RunningBalance\" TYPE numeric USING \"RunningBalance\"::numeric;");

            migrationBuilder.Sql("ALTER TABLE \"Transactions\" ALTER COLUMN \"Date\" TYPE date USING \"Date\"::date;");

            migrationBuilder.Sql("ALTER TABLE \"Transactions\" ALTER COLUMN \"Amount\" TYPE numeric USING \"Amount\"::numeric;");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AccountGuid",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CounterpartyName",
                table: "Transactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CounterpartyType",
                table: "Transactions",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EnrollmentId",
                table: "Enrollments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Enrollments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "InstitutionId",
                table: "Enrollments",
                type: "text",
                nullable: true);

            // Convert BankAccounts.Guid from text to uuid - need to drop default first
            migrationBuilder.Sql("ALTER TABLE \"BankAccounts\" ALTER COLUMN \"Guid\" DROP DEFAULT;");
            migrationBuilder.Sql("ALTER TABLE \"BankAccounts\" ALTER COLUMN \"Guid\" TYPE uuid USING \"Guid\"::uuid;");

            migrationBuilder.AddColumn<Guid>(
                name: "EnrollmentGuid",
                table: "BankAccounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            // Convert Balances.Ledger and Available from text to numeric
            migrationBuilder.Sql("ALTER TABLE \"Balances\" ALTER COLUMN \"Ledger\" TYPE numeric USING \"Ledger\"::numeric;");
            migrationBuilder.Sql("ALTER TABLE \"Balances\" ALTER COLUMN \"Available\" TYPE numeric USING \"Available\"::numeric;");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountGuid",
                table: "Balances",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Guid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments",
                column: "Guid");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountGuid",
                table: "Transactions",
                column: "AccountGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_UserGuid",
                table: "Enrollments",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_EnrollmentGuid",
                table: "BankAccounts",
                column: "EnrollmentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_AccountGuid",
                table: "Balances",
                column: "AccountGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_BankAccounts_AccountGuid",
                table: "Balances",
                column: "AccountGuid",
                principalTable: "BankAccounts",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Enrollments_EnrollmentGuid",
                table: "BankAccounts",
                column: "EnrollmentGuid",
                principalTable: "Enrollments",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Users_UserGuid",
                table: "Enrollments",
                column: "UserGuid",
                principalTable: "Users",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_AccountGuid",
                table: "Transactions",
                column: "AccountGuid",
                principalTable: "BankAccounts",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_BankAccounts_AccountGuid",
                table: "Balances");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Enrollments_EnrollmentGuid",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Users_UserGuid",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_AccountGuid",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountGuid",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_UserGuid",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_EnrollmentGuid",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Balances_AccountGuid",
                table: "Balances");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AccountGuid",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CounterpartyName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CounterpartyType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "EnrollmentGuid",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "AccountGuid",
                table: "Balances");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ProcessingStatus",
                table: "Transactions",
                newName: "BankAccountGuid");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "Enrollments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "AccessToken",
                table: "Enrollments",
                newName: "EncryptedAccessToken");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "RunningBalance",
                table: "Transactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "Transactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Amount",
                table: "Transactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<Dictionary<string, string>>(
                name: "Details",
                table: "Transactions",
                type: "hstore",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EnrollmentId",
                table: "Enrollments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Guid",
                table: "BankAccounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BankAccounts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ledger",
                table: "Balances",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Available",
                table: "Balances",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountGuid",
                table: "Balances",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BankAccountGuid",
                table: "Transactions",
                column: "BankAccountGuid");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_EnrollmentId",
                table: "BankAccounts",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_BankAccountGuid",
                table: "Balances",
                column: "BankAccountGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_BankAccounts_BankAccountGuid",
                table: "Balances",
                column: "BankAccountGuid",
                principalTable: "BankAccounts",
                principalColumn: "Guid");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Enrollments_EnrollmentId",
                table: "BankAccounts",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "EnrollmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Users_UserId",
                table: "BankAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_BankAccountGuid",
                table: "Transactions",
                column: "BankAccountGuid",
                principalTable: "BankAccounts",
                principalColumn: "Guid");
        }
    }
}
