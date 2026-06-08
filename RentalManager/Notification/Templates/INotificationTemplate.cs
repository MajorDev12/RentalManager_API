using RentalManager.Notification.Events;

namespace RentalManager.Notification.Templates
{
    public interface INotificationTemplate
    {
        string EventName { get; }
        string Role { get; }

        string BuildTitle(INotificationEvent @event);
        string BuildBody(INotificationEvent @event);
    }

}
