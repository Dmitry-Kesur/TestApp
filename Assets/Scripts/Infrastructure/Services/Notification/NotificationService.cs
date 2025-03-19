using DG.Tweening;
using Infrastructure.Data.Notifications;
using Infrastructure.Factories.Notification;
using Infrastructure.Providers.UI;
using Infrastructure.Views.UI.Notifications;
using UnityEngine;

namespace Infrastructure.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationsFactory _notificationsFactory;
        private readonly RectTransform _notificationsLayer;

        private OverlayView _notificationsOverlay;
        private NotificationView _activeNotificationView;

        private Tween _animationTween;

        public NotificationService(UIProvider uiProvider, NotificationsFactory notificationsFactory)
        {
            _notificationsLayer = uiProvider.NotificationsLayer;
            _notificationsFactory = notificationsFactory;
            CreateNotificationOverlay();
        }

        public void ShowNotification(NotificationModel notificationModel)
        {
            ShowNotificationsOverlay();

            _activeNotificationView = _notificationsFactory.CreateNotification(notificationModel);
            _activeNotificationView.transform.SetParent(_notificationsLayer, false);
            _activeNotificationView.OnShowNotification(notificationModel);
            notificationModel.CloseNotificationAction = HideNotification;

            _animationTween = DOTween.Sequence()
                .Append(_activeNotificationView.transform.DOScale(1.2f, 0.2f))
                .Append(_activeNotificationView.transform.DOScale(1.5f, 0.2f))
                .Append(_activeNotificationView.transform.DOScale(1f, 0.2f));
        }

        public void HideNotification()
        {
            HideNotificationsOverlay();
            _animationTween = _activeNotificationView.transform.DOScale(0, 0.2f).OnComplete(OnCompleteAnimation);
        }

        private void OnCompleteAnimation()
        {
            Object.Destroy(_activeNotificationView.gameObject);
            _activeNotificationView = null;
                
            _animationTween?.Kill();
            _animationTween = null;
        }

        private void CreateNotificationOverlay()
        {
            _notificationsOverlay = _notificationsFactory.CreateNotificationsOverlay();
            _notificationsOverlay.OverlayButtonClick = HideNotification;
            _notificationsOverlay.transform.SetParent(_notificationsLayer, false);
            HideNotificationsOverlay();
        }

        private void ShowNotificationsOverlay()
        {
            _notificationsOverlay.gameObject.SetActive(true);
        }

        private void HideNotificationsOverlay()
        {
            _notificationsOverlay.gameObject.SetActive(false);
        }
    }
}