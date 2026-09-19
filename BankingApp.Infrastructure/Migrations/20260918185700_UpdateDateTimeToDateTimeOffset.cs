using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateTimeToDateTimeOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Customers_CustomerID",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Accounts_AccountID",
                table: "LedgerEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionID",
                table: "LedgerEntries");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Transactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TransactionID",
                table: "LedgerEntries",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "LedgerEntries",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "LedgerEntries",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_LedgerEntries_TransactionID",
                table: "LedgerEntries",
                newName: "IX_LedgerEntries_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_LedgerEntries_AccountID",
                table: "LedgerEntries",
                newName: "IX_LedgerEntries_AccountId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Customers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Accounts",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Accounts",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_CustomerID",
                table: "Accounts",
                newName: "IX_Accounts_CustomerId");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Timestamp",
                table: "Transactions",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Timestamp",
                table: "LedgerEntries",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateOfBirth",
                table: "Customers",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DateTime");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Customers_CustomerId",
                table: "Accounts",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Accounts_AccountId",
                table: "LedgerEntries",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionId",
                table: "LedgerEntries",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Customers_CustomerId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Accounts_AccountId",
                table: "LedgerEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionId",
                table: "LedgerEntries");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Transactions",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "LedgerEntries",
                newName: "TransactionID");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "LedgerEntries",
                newName: "AccountID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LedgerEntries",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_LedgerEntries_TransactionId",
                table: "LedgerEntries",
                newName: "IX_LedgerEntries_TransactionID");

            migrationBuilder.RenameIndex(
                name: "IX_LedgerEntries_AccountId",
                table: "LedgerEntries",
                newName: "IX_LedgerEntries_AccountID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Customers",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Accounts",
                newName: "CustomerID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Accounts",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_CustomerId",
                table: "Accounts",
                newName: "IX_Accounts_CustomerID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "LedgerEntries",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Customers",
                type: "DateTime",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Customers_CustomerID",
                table: "Accounts",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Accounts_AccountID",
                table: "LedgerEntries",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionID",
                table: "LedgerEntries",
                column: "TransactionID",
                principalTable: "Transactions",
                principalColumn: "ID");
        }
    }
}
