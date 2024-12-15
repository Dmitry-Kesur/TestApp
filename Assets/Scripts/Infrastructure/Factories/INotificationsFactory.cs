using Infrastructure.Data.Notifications;
using Infrastructure.Views.UI.Notifications;

namespace Infrastructure.Factories
{
    public interface INotificationsFactory
    {
        NotificationView CreateNotification(NotificationModel notificationModel);
        OverlayView CreateNotificationsOverlay();
    }
}