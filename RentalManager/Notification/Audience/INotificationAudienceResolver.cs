using RentalManager.Notification.Events;

namespace RentalManager.Notification.Audience
{
    public interface INotificationAudienceResolver
    {
        Task<IReadOnlyList<NotificationRecipient>> ResolveAsync(
            INotificationEvent @event
        );
    }
}
