using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Tenant
{
    public class ASSIGNUnitDto
    {
        public int tenantId {  get; set; }

        public int unitId { get; set; }

        public int statusId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int DepositAmount { get; set; } = 0;

        public int AmountPaid { get; set; }

        public int? PaymentMethodId { get; set; }
    }
}
