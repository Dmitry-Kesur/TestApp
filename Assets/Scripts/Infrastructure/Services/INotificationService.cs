using Infrastructure.Data.Notifications;

namespace Infrastructure.Services
{
    public interface INotificationService
    {
        void ShowNotification(NotificationModel notificationModel);
        void HideNotification();
    }
}