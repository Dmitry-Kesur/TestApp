using Infrastructure.Data.Notifications;
using Infrastructure.Views.UI.Buttons;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Notifications
{
    public class NotificationView : MonoBehaviour
    {
        [SerializeField] private ButtonWithIcon _closeNotificationButton;

        protected NotificationModel NotificationModel;

        public virtual void OnShowNotification(NotificationModel notificationModel)
        {
            NotificationModel = notificationModel;
            _closeNotificationButton.OnButtonClickAction = CloseNotificationAction;
        }

        private void CloseNotificationAction()
        {
            NotificationModel.CloseNotificationAction?.Invoke();
        }
    }
}