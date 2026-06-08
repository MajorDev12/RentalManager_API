using Microsoft.Extensions.DependencyInjection;
using RentalManager.Notification.Audience;
using RentalManager.Notification.Channels;
using RentalManager.Notification.Repositories;
using RentalManager.Notification.Services;
using RentalManager.Notification.Templates;

namespace RentalManager.DI.Notifications
{
    public static class NotificationServiceExtensions
    {
        public static IServiceCollection AddNotificationServices(
            this IServiceCollection services)
        {
            // Core notification services
            services.AddScoped<NotificationService>();
            services.AddScoped<NotificationDispatcher>();

            // Channels
            services.AddScoped<INotificationChannel, InAppNotificationChannel>();

            // Audience resolvers
            services.AddScoped<INotificationAudienceResolver, TenantAddedAudienceResolver>();

            // Templates
            services.AddScoped<INotificationTemplate, TenantAddedOwnerTemplate>();
            services.AddScoped<INotificationTemplate, TenantWelcomeTemplate>();

            // Preferences / persistence
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationPreferenceRepository, NotificationPreferenceRepository>();
            services.AddScoped<NotificationPreferenceSeeder>();


            return services;
        }
    }
}