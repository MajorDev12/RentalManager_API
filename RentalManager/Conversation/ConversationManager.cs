namespace RentalManager.Conversation
{
    public class ConversationManager
    {
        private readonly IConversationStore _store;

        public ConversationManager(IConversationStore store)
        {
            _store = store;
        }

        public Task<ConversationState> GetAsync(string phone)
            => _store.GetAsync(phone);

        public Task SaveAsync(string phone, ConversationState state)
            => _store.SaveAsync(phone, state);

        public Task ClearAsync(string phone)
            => _store.ClearAsync(phone);
    }
}
