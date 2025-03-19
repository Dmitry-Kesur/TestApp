using System.Collections.Generic;
using Infrastructure.Data.Rewards;

namespace Infrastructure.Services.Reward
{
    public interface IReceiveRewardsService
    {
        void ReceiveRewards(List<RewardReceiveData> receiveRewards);
    }
}