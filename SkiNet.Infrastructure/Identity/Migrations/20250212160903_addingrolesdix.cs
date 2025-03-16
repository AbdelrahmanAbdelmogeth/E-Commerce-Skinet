using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceSkinet.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class addingrolesdix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4f714ea2-b89c-4624-9e70-6671b86c2154", null, "Customer", "CUSTOMER" },
                    { "7e42adb3-7fa1-4351-926a-699898eed279", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f714ea2-b89c-4624-9e70-6671b86c2154");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e42adb3-7fa1-4351-926a-699898eed279");
        }
    }
}
