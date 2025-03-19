using System;
using System.Collections.Generic;

namespace Infrastructure.Data.PlayerProgress
{
    [Serializable]
    public class PlayerProgress
    {
        public int ActiveLevel;
        public int BestScore;
        public int SelectedItemId;
        public int CurrencyAmount;
        public int ActiveBoosterId;
        public string UserId;
        public bool MuteSounds;
        public List<int> CompleteLevelIds = new();
        public List<int> UnlockedLevelItemIds = new();
        public List<int> PurchasedInGameProductIds = new();
        public List<string> PendingInAppPurchaseProducts = new();
    }
}