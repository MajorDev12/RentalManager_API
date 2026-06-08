using RentalManager.DTOs.WhatsApp;
using RentalManager.WhatsApp;

namespace RentalManager.Services.WhatsAppService
{
    public interface IWhatsAppService
    {
        Task<WhatsAppResponseDto> RespondAsync(TwilioMessageDto message);
        Task<string> ProcessAsync(string message, WhatsAppContext context);
    }
}
