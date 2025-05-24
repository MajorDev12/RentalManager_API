using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalManager.Models
{
    public class Payment : AuditableEntity
    {
        public int Id { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime PaymentDate { get; set; }

        public string TransactionCode { get; set; } = string.Empty;

        public string? Notes { get; set; }



        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = new();

        public int? UtilityBillId { get; set; }
        public UtilityBill? UtilityBill { get; set; }

        public int? PaymentMethod { get; set; }
        public SystemCodeItem? PaymentMethodItem { get; set; }

    }

}
