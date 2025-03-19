using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;
using Infrastructure.Factories.Purchase;

namespace Infrastructure.Factories.Progress
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
                PurchasedInGameProductIds = new List<int>(),
                PendingInAppPurchaseProducts = new List<string>()
            };

            return playerProgress;
        }
    }
}