using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;
using Infrastructure.Factories.Purchase;

namespace Infrastructure.Factories.Progress
{
    public class ProgressFactory : IProgressFactory
    {
        public Data.PlayerProgress.Progress CreateNewProgress(string userId)
        {
            var playerProgress = new Data.PlayerProgress.Progress
            {
                ActiveLevel = 1,
                UserId = userId,
                CompleteLevelIds = new List<int>(),
                UnlockedLevelItemIds = new List<int>(),
                PurchasedShopProductIds = new List<int>(),
                PendingInAppPurchaseProducts = new List<string>()
            };

            return playerProgress;
        }
    }
}