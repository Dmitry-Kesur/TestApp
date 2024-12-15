using System.Collections.Generic;
using Infrastructure.Constants;
using Infrastructure.Models.GameEntities.Rewards;

namespace Infrastructure.Data.Notifications
{
    public class NotificationWithRewardsModel : NotificationModel
    {
        private readonly List<RewardModel> _rewards;

        public NotificationWithRewardsModel(List<RewardModel> rewards)
        {
            _rewards = rewards;
        }

        public override string Id => NotificationsId.NotificationWithRewards;

        public List<RewardModel> Rewards =>
            _rewards;
    }
}