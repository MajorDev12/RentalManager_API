namespace RentalManager.Notification.Models
{
    public class NotificationPreference
    {
        public int Id { get; set; }

        // Scope
        public int AccountId { get; set; }
        public int? UserId { get; set; } 

        public string EventType { get; set; } = default!;

        // Channels
        public bool InAppEnabled { get; set; } = true;
        public bool SmsEnabled { get; set; } = false;
        public bool EmailEnabled { get; set; } = false;

        public bool IsEnabled { get; set; } = true;

    }
}
