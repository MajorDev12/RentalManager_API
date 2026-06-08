using RentalManager.Services.AccountAccessService;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManager.Models
{
    public class UnitType : AuditableEntity
    {
        public int Id { get; set; }

        public int SystemCodeItemId { get; set; }
        public SystemCodeItem SystemCodeItem { get; set; } = null!;

        public string? Notes { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}
