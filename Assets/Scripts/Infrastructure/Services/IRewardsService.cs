using System.Collections.Generic;
using Infrastructure.Data.Rewards;
using Infrastructure.Models.GameEntities.Rewards;

namespace Infrastructure.Services
{
    public interface IRewardsService
    {
        List<RewardModel> GetRewardsModelsByData(List<RewardReceiveData> receiveRewardsData);
    }
}