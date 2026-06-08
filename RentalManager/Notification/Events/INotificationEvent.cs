namespace RentalManager.Notification.Events
{
    public interface INotificationEvent
    {
        string EventName { get; }
        int AccountId { get; }
        Dictionary<string, object> Data { get; }
    }
}
