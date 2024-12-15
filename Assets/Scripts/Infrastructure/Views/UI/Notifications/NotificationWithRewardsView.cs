using Infrastructure.Data.Notifications;
using Infrastructure.Views.UI.Loaders;
using UnityEngine;

namespace Infrastructure.Views.UI.Notifications
{
    public class NotificationWithRewardsView : NotificationView
    {
        [SerializeField] private RewardsLoader _rewardsLoader;

        public override void OnShowNotification(NotificationModel notificationModel)
        {
            base.OnShowNotification(notificationModel);

            var notificationWithRewardsModel = notificationModel as NotificationWithRewardsModel;
            _rewardsLoader.DrawLoader(notificationWithRewardsModel?.Rewards);
        }
    }
}