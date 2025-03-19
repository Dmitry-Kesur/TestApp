using Infrastructure.Data.Notifications;

namespace Infrastructure.Services.Notification
{
    public interface INotificationService
    {
        void ShowNotification(NotificationModel notificationModel);
        void HideNotification();
    }
}