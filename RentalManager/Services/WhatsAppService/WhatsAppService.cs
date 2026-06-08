using Microsoft.AspNetCore.Identity;
using RentalManager.Authorization;
using RentalManager.Conversation;
using RentalManager.Data;
using RentalManager.DTOs.WhatsApp;
using RentalManager.Intents;
using RentalManager.Intents.Handlers;
using RentalManager.Models;
using RentalManager.Repositories.UserRepository;
using RentalManager.WhatsApp;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

namespace RentalManager.Services.WhatsAppService
{
    public class WhatsAppService : IWhatsAppService
    {
        //private readonly IDictionary<string, string> _users;
        private readonly IUserRepository _userrepository;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IEnumerable<IIntentHandler> _handlers;
        private readonly IIntentResolver _intentResolver;
        private readonly ConversationManager _conversation;
        private readonly IntentAuthorizationService _auth;


        public WhatsAppService(
            IUserRepository userrepository,
            UserManager<ApplicationUser> usermanager,
            IEnumerable<IIntentHandler> handlers,
            IIntentResolver intentResolver,
            ConversationManager conversation,
            IntentAuthorizationService auth)
        {
            _userrepository = userrepository;
            _usermanager = usermanager;
            _handlers = handlers;
            _intentResolver = intentResolver;
            _conversation = conversation;
            _auth = auth;
        }



        public async Task<WhatsAppResponseDto> RespondAsync(TwilioMessageDto message)
        {
            var response = new WhatsAppResponseDto();

            try
            {
                if (message == null || string.IsNullOrWhiteSpace(message.From))
                {
                    response.Success = true;
                    response.Message = "Invalid message received";
                    return response;
                }

                var phoneNumber = normalizePhoneNumber(message.From.Trim());

                var userNumber = await _userrepository.GetByNumberAsync(phoneNumber);

                // Check if number exists in DB
                if (userNumber == null)
                {
                    response.Success = true;
                    response.Role = "Unknown";
                    response.TwiML = GenerateTwiML("Your number is not registered. Please contact admin.");
                    return response;
                }


                var userRole = await _usermanager.GetRolesAsync(userNumber);

                if (userRole == null)
                {
                    response.Success = true;
                    response.Role = "Unknown";
                    response.TwiML = GenerateTwiML("Your Role does not exist. Please contact admin.");
                    return response;
                }


                var role = userRole.First();

                var context = new WhatsAppContext
                {
                    PhoneNumber = phoneNumber,
                    Role = role,
                    UserId = userNumber.Id,
                    AccountId = userNumber.AccountId,
                    UserName = userNumber.FirstName
                };

                var reply = await ProcessAsync(message.Body, context);

                response.TwiML = GenerateTwiML(reply);

                return response;
            }catch(Exception ex)
            {
                return new WhatsAppResponseDto()
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    TwiML = GenerateTwiML("Service not available at the moment")
                };

            }
        }


        public async Task<string> ProcessAsync(
           string message,
           WhatsAppContext context)
        {
            var state = await _conversation.GetAsync(context.PhoneNumber);

            var intent = await ResolveIntent(message);

            if (!_auth.IsAllowed(context.Role, intent.Intent))
                return "Type *help* to see what i can help you with.";

            var handler = _handlers.FirstOrDefault(h => h.Intent == intent.Intent);

            if (handler == null)
                return "I didn’t understand that. Type *help* to see what I can help you with.";

            var reply = await handler.HandleAsync(intent, context, state);

            await _conversation.SaveAsync(context.PhoneNumber, state);

            return reply;
        }

        private async Task<IntentResult> ResolveIntent(string message)
        {

            return await _intentResolver.ResolveAsync(message);
        }


        private string GenerateTwiML(string text)
        {
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message(text);
            return messagingResponse.ToString();
        }


        private string normalizePhoneNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return string.Empty;

            // Remove whatsapp: prefix
            number = number.Replace("whatsapp:", "", StringComparison.OrdinalIgnoreCase);

            // Remove spaces
            number = number.Replace(" ", "");

            // Remove leading +
            if (number.StartsWith("+"))
                number = number.Substring(1);

            // Convert international to local format
            if (number.StartsWith("2547"))
                return "0" + number.Substring(3); // 2547XXXX → 07XXXX

            if (number.StartsWith("2541"))
                return "0" + number.Substring(3); // 2541XXXX → 01XXXX

            // Already local
            if (number.StartsWith("07") || number.StartsWith("01"))
                return number;

            // Unknown format → return as-is (or log)
            return number;
        }

    }
}
