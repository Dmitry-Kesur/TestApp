using System.Collections.Generic;
using Infrastructure.Controllers.Rewards;

namespace Infrastructure.Factories
{
    public interface IReceiveRewardsControllersFactory
    {
        List<BaseReceiveRewardsController> GetRewardsControllers();
    }
}