using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalManager.DTOs.WhatsApp;
using RentalManager.Services.WhatsAppService;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

namespace RentalManager.Controllers
{
    [Route("api/webhooks/whatsapp")]
    public class WhatsAppWebhookController : ControllerBase
    {
        private readonly IWhatsAppService _service;

        public WhatsAppWebhookController(IWhatsAppService service)
        {
            _service = service;
        }


        [Consumes("application/x-www-form-urlencoded")]
        [HttpPost]
        public async Task<IActionResult> ReceiveMessage([FromForm] TwilioMessageDto message)
        {
            try
            {
                var result = await _service.RespondAsync(message);

                if (!result.Success)
                    return BadRequest(result.Message);

                // Twilio expects XML in response
                return Content(result.TwiML, "application/xml");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
