using Infrastructure.Constants;
using UnityEngine;

namespace Infrastructure.Data.Notifications
{
    public class NotificationWithIconModel : NotificationWithTextModel
    {
        public Sprite NotificationIcon;
        
        public override string Id => NotificationsId.NotificationWithIcon;
    }
}