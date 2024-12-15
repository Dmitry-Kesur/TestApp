using Infrastructure.Constants;
using UnityEngine;

namespace Infrastructure.Data.Notifications
{
    public class NotificationWithIconModel : NotificationModel
    {
        public Sprite NotificationIcon;
        
        public override string Id => NotificationsId.NotificationWithIcon;
    }
}