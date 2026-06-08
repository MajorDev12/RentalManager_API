using RentalManager.Notification.Audience;
using RentalManager.Notification.Channels;
using RentalManager.Notification.Defaults;
using RentalManager.Notification.Events;
using RentalManager.Notification.Models;
using RentalManager.Notification.Templates;

namespace RentalManager.Notification.Services
{
    public class NotificationDispatcher
    {
        private readonly IEnumerable<INotificationChannel> _channels;

        public NotificationDispatcher(IEnumerable<INotificationChannel> channels)
        {
            _channels = channels;
        }

        public async Task DispatchAsync(
            INotificationEvent @event,
            NotificationPreference pref,
            INotificationTemplate template,
            NotificationRecipient recipient)
        {
            if (!pref.IsEnabled)
                return;

            foreach (var channel in _channels)
            {
                if (ShouldSend(channel, pref))
                {
                    await channel.SendAsync(
                        @event,
                        pref,
                        template,
                        recipient
                    );
                }
            }
        }

        private static bool ShouldSend(
            INotificationChannel channel,
            NotificationPreference pref)
        {
            return channel.Channel switch
            {
                NotificationConstants.Channel.App => pref.InAppEnabled,
                NotificationConstants.Channel.Sms => pref.SmsEnabled,
                NotificationConstants.Channel.Email => pref.EmailEnabled,
                _ => false
            };
        }
    }



}
