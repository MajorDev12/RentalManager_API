using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalManager.Models
{
    public class Unit : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Amount { get; set; }

        public string? Notes { get; set; }



        public int UnitTypeId { get; set; }
        public UnitType UnitType { get; set; } = null!;

        public int StatusId { get; set; }

        public SystemCodeItem Status { get; set; } = null!;

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }

}
