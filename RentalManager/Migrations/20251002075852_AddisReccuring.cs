using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalManager.Migrations
{
    /// <inheritdoc />
    public partial class AddisReccuring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isReccuring",
                table: "UnitCharges",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isReccuring",
                table: "UnitCharges");
        }
    }
}
