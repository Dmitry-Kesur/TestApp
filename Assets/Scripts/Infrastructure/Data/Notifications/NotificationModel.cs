using System;

namespace Infrastructure.Data.Notifications
{
    public class NotificationModel
    {
        public Action CloseNotificationAction;
        public string NotificationText;

        public virtual string Id => "";
    }
}