using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalManager.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountInProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);


            migrationBuilder.CreateIndex(
                name: "IX_Properties_AccountId",
                table: "Properties",
                column: "AccountId");


            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Accounts_AccountId",
                table: "Properties",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 1, 20, 8, 32, 706, DateTimeKind.Local).AddTicks(8046));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 1, 20, 8, 32, 706, DateTimeKind.Local).AddTicks(8072));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 1, 20, 8, 32, 706, DateTimeKind.Local).AddTicks(8074));
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Accounts_AccountId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_AccountId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Properties");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 12, 31, 9, 55, 24, 657, DateTimeKind.Local).AddTicks(1935));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 12, 31, 9, 55, 24, 657, DateTimeKind.Local).AddTicks(1953));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 12, 31, 9, 55, 24, 657, DateTimeKind.Local).AddTicks(1955));
        }
    }
}
