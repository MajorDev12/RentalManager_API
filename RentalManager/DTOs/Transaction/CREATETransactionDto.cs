using RentalManager.Models;

namespace RentalManager.DTOs.Transaction
{
    public class CREATETransactionDto : AuditableEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? UnitId { get; set; }

        public int? UtilityBillId { get; set; }

        public int TransactionTypeId { get; set; }

        public int TransactionCategoryId { get; set; }

        public int Amount { get; set; }

        public int? PaymentMethodId { get; set; }

        public DateTime TransactionDate { get; set; } 

        public int? MonthFor { get; set; }

        public int? YearFor { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
