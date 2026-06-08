namespace RentalManager.Conversation
{
    public class ConversationState
    {
        public string? LastIntent { get; set; }
        public string? PendingConfirmation { get; set; }
        public Dictionary<string, string> Memory { get; set; } = new();
        public DateTime UpdatedAt { get; set; }
    }
}
