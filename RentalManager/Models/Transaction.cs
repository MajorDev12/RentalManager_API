namespace RentalManager.Models
{
    public class Transaction : AuditableEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int? UnitId { get; set; }

        public Unit? Unit { get; set; }

        public int? UtilityBillId { get; set; }

        public UtilityBill? UtilityBill { get; set; }

        public int TransactionTypeId { get; set; }
        public SystemCodeItem TransactionType { get; set; } = null!;

        public int TransactionCategoryId { get; set; }
        public SystemCodeItem TransactionCategory { get; set; } = null!;

        public int Amount { get; set; } 

        public int? PaymentMethodId { get; set; }

        public SystemCodeItem? PaymentMethod { get; set; }

        public DateTime TransactionDate { get; set; }

        public int? MonthFor { get; set; }

        public int? YearFor { get; set; }

        public string? Notes { get; set; }


    }
}
