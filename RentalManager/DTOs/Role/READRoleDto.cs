namespace RentalManager.DTOs.Role
{
    public class READRoleDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = true;

        public int PropertyId { get; set; }

        public string PropertyName { get; set; } = string.Empty;
    }
}
