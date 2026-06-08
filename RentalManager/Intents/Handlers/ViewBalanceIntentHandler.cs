using RentalManager.Conversation;
using RentalManager.Repositories.TransactionRepository;
using RentalManager.Services.AccountAccessService;
using RentalManager.WhatsApp;

namespace RentalManager.Intents.Handlers
{
    public class ViewBalanceIntentHandler : IIntentHandler
    {

        private readonly ITransactionRepository _transactionRepo;
        private readonly IAccountContext _accountContext;

        public ViewBalanceIntentHandler(ITransactionRepository transactionRepo, IAccountContext accountContext)
        {
            _transactionRepo = transactionRepo;
            _accountContext = accountContext;
        }

        public string Intent => IntentNames.ViewBalance;

        public async Task<string> HandleAsync(
            IntentResult intent,
            WhatsAppContext context,
            ConversationState state)
        {
            // 1️⃣ Authorization check
            if (context.Role is not ("Owner" or "Manager"))
            {
                return "❌ You are not authorized to view tenant balances.";
            }

            Console.WriteLine($"ThIS IS THE ACCOUNT ID : {_accountContext.AccountId}");
            // get Balances
            var balances = await _transactionRepo.GetBalancesAsync();

            if (!balances.Any())
            {
                return "✅ All tenants are fully paid. No outstanding balances.";
            }

            // 3️⃣ Format WhatsApp message (keep it short)
            var message = $"📊 *Outstanding Tenant Balances*\n\n";

            foreach (var item in balances.Take(10)) // avoid huge WhatsApp messages
            {
                message +=
                    $"👤 {item.FullName}\n" +
                    $"🏠 {item.PropertyName} - {item.UnitName}\n" +
                    $"💰 Balance: {item.Balance:C}\n\n";
            }

            if (balances.Count > 10)
            {
                message += $"...and {balances.Count - 10} more tenants.";
            }

            return message;


        }
    }
}
