using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RentalManager.Migrations
{
    /// <inheritdoc />
    public partial class addPropertyAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PropertyAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    AssignmentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyAssignments_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyAssignments_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyAssignments_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Name" },
                values: new object[] { new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1669), "SuperAdmin" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Name" },
                values: new object[] { new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1690), "Owner" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "Name" },
                values: new object[] { new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1691), "Manager" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "IsEnabled", "Name", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 4, null, new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1693), true, "Admin", null, null },
                    { 5, null, new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1694), true, "Landlord", null, null },
                    { 6, null, new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1696), true, "Tenant", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitCharges_AccountId",
                table: "UnitCharges",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAssignments_CreatedBy",
                table: "PropertyAssignments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAssignments_PropertyId",
                table: "PropertyAssignments",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAssignments_UpdatedBy",
                table: "PropertyAssignments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAssignments_UserId_PropertyId",
                table: "PropertyAssignments",
                columns: new[] { "UserId", "PropertyId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitCharges_Accounts_AccountId",
                table: "UnitCharges",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitCharges_Accounts_AccountId",
                table: "UnitCharges");

            migrationBuilder.DropTable(
                name: "PropertyAssignments");

            migrationBuilder.DropIndex(
                name: "IX_UnitCharges_AccountId",
                table: "UnitCharges");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 20, 8, 32, 706, DateTimeKind.Local).AddTicks(8046), "Admin" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 20, 8, 32, 706, DateTimeKind.Local).AddTicks(8072), "Landlord" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 20, 8, 32, 706, DateTimeKind.Local).AddTicks(8074), "Tenant" });
        }
    }
}
