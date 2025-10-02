using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalManager.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseRemoveTransactionCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_SystemCodeItems_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expense_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ExpenseId",
                table: "Transactions",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_CreatedBy",
                table: "Expense",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_UpdatedBy",
                table: "Expense",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Expense_ExpenseId",
                table: "Transactions",
                column: "ExpenseId",
                principalTable: "Expense",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Expense_ExpenseId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ExpenseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "TransactionCategoryId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionCategoryId",
                table: "Transactions",
                column: "TransactionCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_SystemCodeItems_TransactionCategoryId",
                table: "Transactions",
                column: "TransactionCategoryId",
                principalTable: "SystemCodeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
