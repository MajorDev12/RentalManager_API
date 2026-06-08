namespace RentalManager.DTOs.WhatsApp
{
    public class WhatsAppResponseDto
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public string TwiML { get; set; } = string.Empty; // XML response
        public string Role { get; set; } = string.Empty; // Landlord/Tenant
    }
}
