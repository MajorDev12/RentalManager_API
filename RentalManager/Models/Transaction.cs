using RentalManager.Services.AccountAccessService;

namespace RentalManager.Models
{
    public class Transaction : AuditableEntity
    {
        public int Id { get; set; }

        public string TransactionNumber { get; set; } = null!;

        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public int? UserId { get; set; }
        public User? User { get; set; }


        public int? PropertyId { get; set; }
        public Property? Property { get; set; }


        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }

        public int? ReferenceId { get; set; }
        public string? ReferenceType { get; set; }



        public int TransactionTypeId { get; set; }
        public SystemCodeItem TransactionType { get; set; } = null!;


        public int TransactionCategoryId { get; set; }
        public SystemCodeItem TransactionCategory { get; set; } = null!;


        public decimal Amount { get; set; } 


        public int? PaymentMethodId { get; set; }
        public SystemCodeItem? PaymentMethod { get; set; }


        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;


        public int? MonthFor { get; set; }


        public int? YearFor { get; set; }


        public string? Notes { get; set; }

        public ICollection<TransactionAllocation> PaymentAllocations { get; set; } = new List<TransactionAllocation>();
        public ICollection<TransactionAllocation> ChargeAllocations { get; set; } = new List<TransactionAllocation>();

        public ICollection<TransactionRelation> FromRelations { get; set; } = new List<TransactionRelation>();
        public ICollection<TransactionRelation> ToRelations { get; set; } = new List<TransactionRelation>();

    }
}
