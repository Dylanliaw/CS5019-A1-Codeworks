using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CS5019_A1_Codeworks.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6164af3d-4218-4e49-b7a6-4ff0c6f86e8d", null, "seller", "seller" },
                    { "c1b01ddf-0d06-4855-9fa2-f1411cc1909e", null, "client", "client" },
                    { "f77c60a8-40a4-401c-83f7-06f8726a4509", null, "admin", "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6164af3d-4218-4e49-b7a6-4ff0c6f86e8d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1b01ddf-0d06-4855-9fa2-f1411cc1909e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f77c60a8-40a4-401c-83f7-06f8726a4509");
        }
    }
}
