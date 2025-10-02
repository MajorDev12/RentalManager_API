using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalManager.Models
{
    public class UtilityBill : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public bool isReccuring { get; set; } = true;

        public int PropertyId { get; set; }

        public Property? Property { get; set; }

    }

}
