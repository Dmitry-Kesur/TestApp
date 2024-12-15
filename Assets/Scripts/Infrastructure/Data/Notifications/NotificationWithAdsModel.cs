using System;
using Infrastructure.Constants;

namespace Infrastructure.Data.Notifications
{
    public class NotificationWithAdsModel : NotificationModel
    {
        public Action ShowAdsAction;
        public override string Id => NotificationsId.NotificationWithAds;
    }
}