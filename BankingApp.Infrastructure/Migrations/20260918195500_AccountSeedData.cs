using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AccountSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountNumber", "Currency", "CustomerId", "Type" },
                values: new object[,]
                {
                    { new Guid("06320057-2872-42af-a09a-6635ee881ddb"), "990888064302", "USD", new Guid("7222f4a6-b8ca-4cc5-ad74-7b4b34d79779"), 1 },
                    { new Guid("29feb7be-21e0-4f55-90ca-df47379569ba"), "416459422322", "USD", new Guid("afbd278c-5176-4510-9ad1-ae2acf34f80a"), 0 },
                    { new Guid("360b7962-1b37-4880-a87e-c244d1001e52"), "693762756416", "USD", new Guid("7222f4a6-b8ca-4cc5-ad74-7b4b34d79779"), 0 },
                    { new Guid("5ccf1509-a6f3-42d2-9cc7-3b3fab726300"), "228947707340", "USD", new Guid("418586f3-7268-4035-8ae7-d2241c474876"), 0 },
                    { new Guid("8a0e9a6a-0ea3-4439-9fdc-20860f836f42"), "145967913707", "USD", new Guid("afbd278c-5176-4510-9ad1-ae2acf34f80a"), 1 },
                    { new Guid("b131623b-44ec-4e60-84e2-23ee5638e28d"), "752142281253", "USD", new Guid("4bf2a680-ac31-4206-be1a-8effc3d6c427"), 1 },
                    { new Guid("d48b9479-c96f-4402-a356-4d2f830aab71"), "660279434684", "USD", new Guid("4bf2a680-ac31-4206-be1a-8effc3d6c427"), 0 },
                    { new Guid("e65ae612-bc01-431a-ad1f-7d6c19a74e34"), "681627673954", "USD", new Guid("418586f3-7268-4035-8ae7-d2241c474876"), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("06320057-2872-42af-a09a-6635ee881ddb"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("29feb7be-21e0-4f55-90ca-df47379569ba"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("360b7962-1b37-4880-a87e-c244d1001e52"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("5ccf1509-a6f3-42d2-9cc7-3b3fab726300"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("8a0e9a6a-0ea3-4439-9fdc-20860f836f42"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("b131623b-44ec-4e60-84e2-23ee5638e28d"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("d48b9479-c96f-4402-a356-4d2f830aab71"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("e65ae612-bc01-431a-ad1f-7d6c19a74e34"));
        }
    }
}
