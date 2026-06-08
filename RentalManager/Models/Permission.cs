namespace RentalManager.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
    }
}
