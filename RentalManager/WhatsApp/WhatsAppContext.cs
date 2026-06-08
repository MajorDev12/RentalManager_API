namespace RentalManager.WhatsApp
{
    public class WhatsAppContext
    {
        public int UserId { get; init; }
        public string UserName { get; init; } = null!;
        public int AccountId { get; init; }
        public string Role { get; init; } = null!;
        public string PhoneNumber { get; init; } = null!;
    }
}
