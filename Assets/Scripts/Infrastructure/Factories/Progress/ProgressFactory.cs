using System.Collections.Generic;
using Infrastructure.Data;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Factories.Progress
{
    public class ProgressFactory : IProgressFactory
    {
        public ProgressData CreateProgress(string userId)
        {
            var playerProgress = new ProgressData
            {
                ActiveLevel = 1,
                UserId = userId,
                WinLevelIds = new List<int>(),
                UnlockedLevelItemIds = new List<int>(),
                PurchasedShopProductIds = new List<int>(),
                Resources = new List<ProgressResourceData>()
            };

            return playerProgress;
        }
    }
}