using Infrastructure.Constants;

namespace Infrastructure.Data.Notifications
{
    public class NotificationWithTextModel : NotificationModel
    {
        public string NotificationText;

        public override string Id => NotificationsId.NotificationWithText;
    }
}