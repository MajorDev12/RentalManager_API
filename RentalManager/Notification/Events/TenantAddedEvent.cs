namespace RentalManager.Notification.Events
{
    public class TenantAddedEvent : INotificationEvent
    {
        public string EventName => NotificationEventNames.TenantAdded;

        public int AccountId { get; init; }
        public int PropertyId { get; init; }
        public int TenantUserId { get; init; }

        public Dictionary<string, object> Data => new()
        {
            ["PropertyId"] = PropertyId,
            ["TenantUserId"] = TenantUserId
        };
    }

}
