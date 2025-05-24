using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.UtilityBill
{
    public class CREATEUtilityBillDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int PropertyId { get; set; }
    }
}
