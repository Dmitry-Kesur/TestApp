using System.Collections.Generic;
using Infrastructure.Controllers.Rewards;

namespace Infrastructure.Factories.Reward
{
    public interface IReceiveRewardsControllersFactory
    {
        List<BaseReceiveRewardsController> GetRewardsControllers();
    }
}