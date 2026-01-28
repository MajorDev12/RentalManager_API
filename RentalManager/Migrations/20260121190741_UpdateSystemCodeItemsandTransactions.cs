using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RentalManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSystemCodeItemsandTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Properties_PropertyId",
                table: "Expenses");


            migrationBuilder.AddColumn<int>(
                name: "ExpenseCategoryId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(1839));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2042));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2046));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2048));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2050));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2052));

            migrationBuilder.InsertData(
                table: "SystemCodeItems",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Item", "Notes", "SystemCodeId", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1041, null, new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2203), "Maintenance", "House Maintenance", 16, null, null },
                    { 1042, null, new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2214), "Salary", "Staff Salary Payments", 16, null, null },
                    { 1043, null, new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2216), "Cleaning", "Cleaning Services", 16, null, null },
                    { 1044, null, new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2219), "Insurance", "Property Insurance Costs", 16, null, null },
                    { 1045, null, new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2221), "Legal", "Legal Fees and Services", 16, null, null },
                    { 1046, null, new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2240), "Marketing", "Advertising and Promotion Costs", 16, null, null },
                    { 1047, null, new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2245), "Office Supplies", "Office and Administrative Supplies", 16, null, null },
                    { 1048, null, new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2248), "Security", "Security and Surveillance Expenses", 16, null, null },
                    { 1049, null, new DateTime(2026, 1, 21, 22, 7, 38, 399, DateTimeKind.Local).AddTicks(2250), "Other", "Other expenses not classified elsewhere", 16, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitTypes_AccountId",
                table: "UnitTypes",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ExpenseCategoryId",
                table: "Transactions",
                column: "ExpenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AccountId",
                table: "Expenses",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Accounts_AccountId",
                table: "Expenses",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Properties_PropertyId",
                table: "Expenses",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Expenses_ExpenseId",
                table: "Transactions",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_SystemCodeItems_ExpenseCategoryId",
                table: "Transactions",
                column: "ExpenseCategoryId",
                principalTable: "SystemCodeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitTypes_Accounts_AccountId",
                table: "UnitTypes",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Accounts_AccountId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Properties_PropertyId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Expenses_ExpenseId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_SystemCodeItems_ExpenseCategoryId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitTypes_Accounts_AccountId",
                table: "UnitTypes");

            migrationBuilder.DropIndex(
                name: "IX_UnitTypes_AccountId",
                table: "UnitTypes");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ExpenseCategoryId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_AccountId",
                table: "Expenses");

            migrationBuilder.DeleteData(
                table: "SystemCodeItems",
                keyColumn: "Id",
                keyValue: 1041);

            migrationBuilder.DeleteData(
                table: "SystemCodeItems",
                keyColumn: "Id",
                keyValue: 1042);

            migrationBuilder.DeleteData(
                table: "SystemCodeItems",
                keyColumn: "Id",
                keyValue: 1043);

            migrationBuilder.DeleteData(
                table: "SystemCodeItems",
                keyColumn: "Id",
                keyValue: 1044);

            migrationBuilder.DeleteData(
                table: "SystemCodeItems",
                keyColumn: "Id",
                keyValue: 1045);

            migrationBuilder.DeleteData(
                table: "SystemCodeItems",
                keyColumn: "Id",
                keyValue: 1046);

            migrationBuilder.DeleteData(
                table: "SystemCodeItems",
                keyColumn: "Id",
                keyValue: 1047);

            migrationBuilder.DeleteData(
                table: "SystemCodeItems",
                keyColumn: "Id",
                keyValue: 1048);

            migrationBuilder.DeleteData(
                table: "SystemCodeItems",
                keyColumn: "Id",
                keyValue: 1049);

            migrationBuilder.DropColumn(
                name: "ExpenseCategoryId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1669));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1690));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1691));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1693));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1694));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2026, 1, 10, 15, 57, 7, 65, DateTimeKind.Local).AddTicks(1696));

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Properties_PropertyId",
                table: "Expenses",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Expenses_ExpenseId",
                table: "Transactions",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
