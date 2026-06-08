namespace RentalManager.Conversation
{
    public interface IConversationStore
    {
        Task<ConversationState> GetAsync(string phone);
        Task SaveAsync(string phone, ConversationState state);
        Task ClearAsync(string phone);
    }
}
