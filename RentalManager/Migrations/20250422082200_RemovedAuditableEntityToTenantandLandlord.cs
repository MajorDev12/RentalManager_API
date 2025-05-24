using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalManager.Migrations
{
    /// <inheritdoc />
    public partial class RemovedAuditableEntityToTenantandLandlord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Landlords_Users_CreatedBy",
                table: "Landlords");

            migrationBuilder.DropForeignKey(
                name: "FK_Landlords_Users_UpdatedBy",
                table: "Landlords");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Users_CreatedBy",
                table: "Tenants");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Users_UpdatedBy",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_CreatedBy",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_UpdatedBy",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Landlords_CreatedBy",
                table: "Landlords");

            migrationBuilder.DropIndex(
                name: "IX_Landlords_UpdatedBy",
                table: "Landlords");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Landlords");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Landlords");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Landlords");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Landlords");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedBy",
                table: "Users",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UpdatedBy",
                table: "Users",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatedBy",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UpdatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UpdatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Tenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Tenants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Tenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Tenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Landlords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Landlords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Landlords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Landlords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CreatedBy",
                table: "Tenants",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_UpdatedBy",
                table: "Tenants",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Landlords_CreatedBy",
                table: "Landlords",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Landlords_UpdatedBy",
                table: "Landlords",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Landlords_Users_CreatedBy",
                table: "Landlords",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Landlords_Users_UpdatedBy",
                table: "Landlords",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Users_CreatedBy",
                table: "Tenants",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Users_UpdatedBy",
                table: "Tenants",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
