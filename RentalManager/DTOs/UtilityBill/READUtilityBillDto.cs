using System.ComponentModel.DataAnnotations;
using RentalManager.Models;

namespace RentalManager.DTOs.UtilityBill
{
    public class READUtilityBillDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public int PropertyId { get; set; }

        public string PropertyName { get; set; } = string.Empty;
    }
}
