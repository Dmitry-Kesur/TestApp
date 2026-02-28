using Infrastructure.Data.Notifications;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Notifications
{
    public class NotificationWithTextView : NotificationView
    {
        [SerializeField] private TextMeshProUGUI _notificationTextField;

        public override void OnShowNotification(NotificationModel notificationModel)
        {
            base.OnShowNotification(notificationModel);
            _notificationTextField.text = ((NotificationWithTextModel)NotificationModel).NotificationText;
        }
    }
}