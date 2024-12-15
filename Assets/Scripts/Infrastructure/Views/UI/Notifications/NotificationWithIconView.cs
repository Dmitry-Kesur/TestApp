using Infrastructure.Data.Notifications;
using Infrastructure.Views.UI.Loaders;
using UnityEngine;

namespace Infrastructure.Views.UI.Notifications
{
    public class NotificationWithIconView : NotificationView
    {
        [SerializeField] private IconLoader _icon;

        public override void OnShowNotification(NotificationModel notificationModel)
        {
            base.OnShowNotification(notificationModel);
            var notificationWithIconModel = notificationModel as NotificationWithIconModel;
            _icon.SetIconSprite(notificationWithIconModel?.NotificationIcon);
        }
    }
}