using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StatusColumnForAccountAndCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerStatus",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountStatus",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
            }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerStatus",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "Accounts");
        }
    }
}
