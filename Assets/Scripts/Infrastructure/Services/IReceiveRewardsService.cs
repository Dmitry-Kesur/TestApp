using System.Collections.Generic;
using Infrastructure.Data.Rewards;

namespace Infrastructure.Services
{
    public interface IReceiveRewardsService
    {
        void ReceiveRewards(List<RewardReceiveData> receiveRewards);
    }
}