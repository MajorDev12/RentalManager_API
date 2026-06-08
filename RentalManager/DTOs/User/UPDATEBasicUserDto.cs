namespace RentalManager.DTOs.User
{
    public class UPDATEBasicUserDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? UserName { get; set; }

        public string? EmailAddress { get; set; }

        public string MobileNumber { get; set; } = string.Empty;

        public string? AlternativeNumber { get; set; }

    }
}
