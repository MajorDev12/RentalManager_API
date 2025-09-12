using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Tenant
{
    public class ASSIGNUnitDto
    {
        [Required]
        public int tenantId {  get; set; }

        [Required]
        public int unitId { get; set; }

        [Required]
        public int status { get; set; }

        public DateTime PaymentDate { get; set; }

        public int DepositAmount { get; set; } = 0;

        public int AmountPaid { get; set; }

        public int? PaymentMethodId { get; set; }
    }
}
