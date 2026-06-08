using RentalManager.DTOs.WhatsApp;
using RentalManager.Services.WhatsAppService;

namespace RentalManager.WhatsApp
{
    public class WhatsAppMessageRouter
    {
        private readonly IWhatsAppService _whatsAppService;

        public WhatsAppMessageRouter(IWhatsAppService whatsAppService)
        {
            _whatsAppService = whatsAppService;
        }

        public async Task<WhatsAppResponseDto> RouteAsync(TwilioMessageDto dto)
        {
            return await _whatsAppService.RespondAsync(dto);
        }
    }
}
