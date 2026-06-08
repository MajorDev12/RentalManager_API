using RentalManager.Notification.Defaults;
using RentalManager.Notification.Events;

namespace RentalManager.Notification.Templates
{
    public class TenantAddedOwnerTemplate : INotificationTemplate
    {
        public string EventName => NotificationEventNames.TenantAdded;
        public string Role => NotificationConstants.Role.Owner;

        public string BuildTitle(INotificationEvent e)
            => "New tenant added";

        public string BuildBody(INotificationEvent e)
        {
            var ev = (TenantAddedEvent)e;
            return $"A new tenant was added to property #{ev.PropertyId}.";
        }
    }

}
