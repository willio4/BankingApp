using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CustomerSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("418586f3-7268-4035-8ae7-d2241c474876"), new DateTimeOffset(new DateTime(1997, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -7, 0, 0, 0)), "omarwilliams@gmail.com", "Omar", "Williams", "111-111-1111" },
                    { new Guid("4bf2a680-ac31-4206-be1a-8effc3d6c427"), new DateTimeOffset(new DateTime(2010, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -8, 0, 0, 0)), "harlemwilliams@gmail.com", "Harlem", "Williams", "222-222-2222" },
                    { new Guid("7222f4a6-b8ca-4cc5-ad74-7b4b34d79779"), new DateTimeOffset(new DateTime(1995, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -7, 0, 0, 0)), "tylerrowlette@gmail.com", "Tyler", "Rowlette", "333-333-3333" },
                    { new Guid("afbd278c-5176-4510-9ad1-ae2acf34f80a"), new DateTimeOffset(new DateTime(2005, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -7, 0, 0, 0)), "jerseyrowlette@gmail.com", "Jerey", "Rowlette", "444-444-4444" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("418586f3-7268-4035-8ae7-d2241c474876"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("4bf2a680-ac31-4206-be1a-8effc3d6c427"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("7222f4a6-b8ca-4cc5-ad74-7b4b34d79779"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("afbd278c-5176-4510-9ad1-ae2acf34f80a"));
        }
    }
}
