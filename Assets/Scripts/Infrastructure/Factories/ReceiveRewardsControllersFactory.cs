using System;
using System.Collections.Generic;
using Infrastructure.Controllers.Rewards;
using Zenject;

namespace Infrastructure.Factories
{
    public class ReceiveRewardsControllersFactory : IReceiveRewardsControllersFactory
    {
        private static readonly List<Type> RewardControllerTypes = new() {typeof(CurrencyReceiveRewardsController)};
        
        private readonly DiContainer _diContainer;

        public ReceiveRewardsControllersFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public List<BaseReceiveRewardsController> GetRewardsControllers()
        {
            List<BaseReceiveRewardsController> rewardsControllers = new();
            
            foreach (var rewardControllerType in RewardControllerTypes)
            {
                var rewardsController = (BaseReceiveRewardsController)_diContainer.Instantiate(rewardControllerType);
                rewardsControllers.Add(rewardsController);
            }

            return rewardsControllers;
        }
    }
}