namespace RentalManager.DTOs.Role
{
    public class UPDATERoleDto
    {
        public string Name { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = true;

        public int PropertyId { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
