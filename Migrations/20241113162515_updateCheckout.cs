using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CS5019_A1_Codeworks.Migrations
{
    /// <inheritdoc />
    public partial class updateCheckout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Modify CVV column to remove length restriction
            migrationBuilder.AlterColumn<string>(
                name: "CVV",
                table: "Checkouts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);

            // Optionally add more changes to your Checkout table here if needed
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the CVV column back to the original state with length restriction
            migrationBuilder.AlterColumn<string>(
                name: "CVV",
                table: "Checkouts",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Optionally revert any other Checkout table changes here
        }
    }
}
