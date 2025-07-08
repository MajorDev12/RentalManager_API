using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Tenant
{
    public class ASSIGNUnitDto
    {
        [Required]
        public int tenantId {  get; set; }

        [Required]
        public int unitId { get; set; }
    }
}
