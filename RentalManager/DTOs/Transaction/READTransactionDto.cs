using RentalManager.DTOs.User;

namespace RentalManager.DTOs.Transaction
{
    public class READTransactionDto
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string? UserName { get; set; }

        public int PropertyId { get; set; }

        public string PropertyName { get; set; } = string.Empty;

        public int? UnitId { get; set; }

        public string? Unit { get; set; }

        public int? UtilityBillId { get; set; }

        public string? UtilityBill { get; set; }

        public int TransactionTypeId { get; set; }
        public string TransactionType { get; set; } = string.Empty;

        public int TransactionCategoryId { get; set; }
        public string TransactionCategory { get; set; } = string.Empty;

        public int Amount { get; set; }

        public int? PaymentMethodId { get; set; }

        public string? PaymentMethod { get; set; }

        public DateTime TransactionDate { get; set; }

        public int MonthFor { get; set; }

        public int YearFor { get; set; }

        public string? Notes { get; set; }
    }
}
