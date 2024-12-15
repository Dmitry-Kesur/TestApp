using Infrastructure.Data.Notifications;
using Infrastructure.Views.UI.Buttons;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Notifications
{
    public class NotificationView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _notificationTextField;
        [SerializeField] private ButtonWithIcon _closeNotificationButton;

        protected NotificationModel NotificationModel;

        public virtual void OnShowNotification(NotificationModel notificationModel)
        {
            this.NotificationModel = notificationModel;
            _closeNotificationButton.OnButtonClickAction = CloseNotificationAction;
            
            _notificationTextField.text = this.NotificationModel.NotificationText;
        }

        private void CloseNotificationAction()
        {
            NotificationModel.CloseNotificationAction?.Invoke();
        }
    }
}