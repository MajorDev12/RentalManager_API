using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalManager.Migrations
{
    public partial class ChangedUnitStatusToStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add new column (nullable first)
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Units",
                type: "int",
                nullable: true);

            // Step 2: Map old Status values to SystemCodeItems
            migrationBuilder.Sql(@"
                UPDATE Units
                SET StatusId = sci.Id
                FROM Units u
                INNER JOIN SystemCodeItems sci ON sci.Item = u.Status
            ");

            // Step 3: Drop old column
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Units");

            // Step 4: Alter StatusId to NOT NULL (if every row has been mapped)
            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Units",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            // Step 5: Create index + FK
            migrationBuilder.CreateIndex(
                name: "IX_Units_StatusId",
                table: "Units",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_SystemCodeItems_StatusId",
                table: "Units",
                column: "StatusId",
                principalTable: "SystemCodeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_SystemCodeItems_StatusId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_StatusId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Units");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Units",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
