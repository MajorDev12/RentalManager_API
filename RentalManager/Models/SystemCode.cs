using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManager.Models
{
    public class SystemCode : AuditableEntity
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string? Notes { get; set; }

    
        //Navigation Property
        public ICollection<SystemCodeItem> SystemCodeItems { get; set; } = new List<SystemCodeItem>();

    }
}
