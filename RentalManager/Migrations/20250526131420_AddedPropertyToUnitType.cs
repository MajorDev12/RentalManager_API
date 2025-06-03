using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyToUnitType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "UnitTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UnitTypes_PropertyId",
                table: "UnitTypes",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitTypes_Properties_PropertyId",
                table: "UnitTypes",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitTypes_Properties_PropertyId",
                table: "UnitTypes");

            migrationBuilder.DropIndex(
                name: "IX_UnitTypes_PropertyId",
                table: "UnitTypes");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "UnitTypes");
        }
    }
}
