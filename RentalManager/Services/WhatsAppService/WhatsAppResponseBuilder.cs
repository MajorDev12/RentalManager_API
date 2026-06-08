using Twilio.TwiML;

namespace RentalManager.Services.WhatsAppService
{
    public class WhatsAppResponseBuilder
    {
        public static string Text(string message)
        {
            var response = new MessagingResponse();
            response.Message(message);
            return response.ToString();
        }
    }
}
