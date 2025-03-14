using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Factories
{
    public class ProgressFactory : IProgressFactory
    {
        public PlayerProgress CreateNewProgress(string userId)
        {
            var playerProgress = new PlayerProgress
            {
                ActiveLevel = 1,
                UserId = userId,
                CompleteLevelIds = new List<int>(),
                UnlockedLevelItemIds = new List<int>(),
                PurchasedProductIds = new List<int>()
            };

            return playerProgress;
        }
    }
}