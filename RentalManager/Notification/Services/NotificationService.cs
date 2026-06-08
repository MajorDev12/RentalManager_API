using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalManager.Models;
using RentalManager.Notification.Audience;
using RentalManager.Notification.Events;
using RentalManager.Notification.Models;
using RentalManager.Notification.Repositories;
using RentalManager.Notification.Services;
using RentalManager.Notification.Templates;
using RentalManager.Services.AccountAccessService;

public class NotificationService
{
    private readonly INotificationAudienceResolver _audienceResolver;
    private readonly INotificationPreferenceRepository _preferenceRepo;
    private readonly IEnumerable<INotificationTemplate> _templates;
    private readonly NotificationDispatcher _dispatcher;
    private readonly INotificationRepository _notificationRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUser _currentUser;

    public NotificationService(
        INotificationAudienceResolver audienceResolver,
        INotificationPreferenceRepository preferenceRepo,
        IEnumerable<INotificationTemplate> templates,
        NotificationDispatcher dispatcher,
        INotificationRepository notificationRepo,
        UserManager<ApplicationUser> userManager,
        ICurrentUser currentUser)
    {
        _audienceResolver = audienceResolver;
        _preferenceRepo = preferenceRepo;
        _templates = templates;
        _dispatcher = dispatcher;
        _notificationRepo = notificationRepo;
        _userManager = userManager;
        _currentUser = currentUser;
    }

    public async Task NotifyAsync(INotificationEvent @event)
    {
        try
        {
            var recipients = await _audienceResolver.ResolveAsync(@event);

            foreach (var recipient in recipients)
            {
                var pref = await _preferenceRepo
                    .GetPreferencesForEventAsync(
                        @event.EventName,
                        @event.AccountId,
                        recipient.UserId);

                if (pref == null || !pref.IsEnabled)
                    continue;

                var template = _templates.First(t =>
                    t.EventName == @event.EventName &&
                    t.Role.Equals(recipient.Role, StringComparison.OrdinalIgnoreCase));

                await _dispatcher.DispatchAsync(@event, pref, template, recipient);

            }

        }
        catch (Exception ex)
        {
            throw;
        }

    }


    public async Task<ApiResponse<IReadOnlyList<InAppNotification>>> GetUnRead(int userId)
    {
        try
        {
            var user = await _userManager.Users
                .Include(u => u.User)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var notification =  await _notificationRepo.GetUnreadAsync(user.User.Id);

            return ApiResponse<IReadOnlyList<InAppNotification>>.SuccessResponse(notification, "Fetched Successfuly");
        }
        catch (Exception ex) 
        {
            return ApiResponse<IReadOnlyList<InAppNotification>>.FailResponse($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
        }
    }


    public async Task<ApiResponse<IReadOnlyList<InAppNotification>>> GetAll()
    {
        try
        {
            int userId = _currentUser.UserId;

            var notification = await _notificationRepo.GetAllAsync(userId);

            return ApiResponse<IReadOnlyList<InAppNotification>>.SuccessResponse(notification, "Fetched Successfuly");
        }
        catch (Exception ex)
        {
            return ApiResponse<IReadOnlyList<InAppNotification>>.FailResponse($"Error Occurred: {ex.InnerException?.Message ?? ex.Message}");
        }
    }

}
