using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.UtilityBill
{
    public class UPDATEUtilityBillDto
    {
        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public bool isReccuring { get; set; }

        public int PropertyId { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
