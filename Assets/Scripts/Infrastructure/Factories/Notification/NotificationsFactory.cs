using Infrastructure.Constants;
using Infrastructure.Data.Notifications;
using Infrastructure.Services;
using Infrastructure.Views.UI.Notifications;

namespace Infrastructure.Factories.Notification
{
    public class NotificationsFactory
    {
        private readonly PrefabInstantiationService _prefabInstantiationService;
        
        public NotificationsFactory(PrefabInstantiationService prefabInstantiationService)
        {
            _prefabInstantiationService = prefabInstantiationService;
        }
        
        public NotificationView CreateNotification(NotificationModel notificationModel)
        {
            NotificationView notificationView = null;

            if (notificationModel.Id == NotificationsId.NotificationWithAds)
            {
                notificationView =
                    _prefabInstantiationService.GetPrefabInstance<NotificationWithAdsView>(NotificationPrefabsPath.NotificationWithAdsPath);
            }

            else if (notificationModel.Id == NotificationsId.NotificationWithIcon)
            {
                notificationView = _prefabInstantiationService.GetPrefabInstance<NotificationWithIconView>(NotificationPrefabsPath.NotificationWithIcon);
            }
            else if (notificationModel.Id == NotificationsId.NotificationWithRewards)
            {
                notificationView = _prefabInstantiationService.GetPrefabInstance<NotificationWithRewardsView>(NotificationPrefabsPath.NotificationWithRewards);
            }

            return notificationView;
        }

        public OverlayView CreateNotificationsOverlay()
        {
            var overlayPrefab = _prefabInstantiationService.GetPrefabInstance<OverlayView>(NotificationPrefabsPath.NotificationsOverlay);
            return overlayPrefab;
        }
    }
}