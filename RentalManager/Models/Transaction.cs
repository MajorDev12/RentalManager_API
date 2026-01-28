using RentalManager.Services.AccountAccessService;

namespace RentalManager.Models
{
    public class Transaction : AuditableEntity, IAccountContext
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public int? UserId { get; set; }
        public User? User { get; set; }


        public int? PropertyId { get; set; }
        public Property? Property { get; set; }


        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }


        public int? UtilityBillId { get; set; }
        public UtilityBill? UtilityBill { get; set; }


        public int? ExpenseId { get; set; }
        public Expense? Expenses { get; set; }


        public int? ExpenseCategoryId { get; set; }
        public SystemCodeItem? ExpenseCategory { get; set; } 


        public int TransactionTypeId { get; set; }
        public SystemCodeItem TransactionType { get; set; } = null!;


        public int TransactionCategoryId { get; set; }
        public SystemCodeItem TransactionCategory { get; set; } = null!;


        public decimal Amount { get; set; } 


        public int? PaymentMethodId { get; set; }
        public SystemCodeItem? PaymentMethod { get; set; }


        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;


        public int MonthFor { get; set; }


        public int YearFor { get; set; }


        public string? Notes { get; set; }


        public bool Combine { get; set; } = true;


        public string? Status { get; set; }

    }
}
