using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalManager.Models
{
    public class SystemLog : AuditableEntity
    {
        public int Id { get; set; }

        public string Action { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public string? IpAddress { get; set; }



        public int UserId { get; set; }
        public User User { get; set; } = new();

        public int LogLevel { get; set; }
        public SystemCodeItem LogLevelStatus { get; set; } = new();
    }

}
