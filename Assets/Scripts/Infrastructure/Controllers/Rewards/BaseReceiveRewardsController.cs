using Infrastructure.Data.Rewards;
using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Rewards;

namespace Infrastructure.Controllers.Rewards
{
    public abstract class BaseReceiveRewardsController
    {
        public virtual RewardType RewardType { get; }

        public abstract void ReceiveReward(RewardModel rewardModel);
    }
}