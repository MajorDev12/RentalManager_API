namespace RentalManager.Notification.Defaults
{
    public class NotificationDefault
    {
        public string EventType { get; init; } = default!;
        public string Role { get; init; } = default!;

        public bool InAppEnabled { get; init; } = true;
        public bool SmsEnabled { get; init; } = false;
        public bool EmailEnabled { get; init; } = false;
    }
}
