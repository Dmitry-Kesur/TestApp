using System;

namespace Infrastructure.Data.Notifications
{
    public class NotificationModel
    {
        public Action CloseNotificationAction;

        public virtual string Id => "";
    }
}