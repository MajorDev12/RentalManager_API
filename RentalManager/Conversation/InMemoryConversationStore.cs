using System.Collections.Concurrent;

namespace RentalManager.Conversation
{
    public class InMemoryConversationStore : IConversationStore
    {
        // Thread-safe storage
        private static readonly ConcurrentDictionary<string, ConversationState> _store
            = new();

        public Task ClearAsync(string phone)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationState> GetAsync(string phoneNumber)
        {
            var state = _store.GetOrAdd(
                phoneNumber,
                _ => new ConversationState()
            );

            return Task.FromResult(state);
        }

        public Task SaveAsync(string phoneNumber, ConversationState state)
        {
            state.UpdatedAt = DateTime.UtcNow;
            _store[phoneNumber] = state;

            return Task.CompletedTask;
        }
    }
}
