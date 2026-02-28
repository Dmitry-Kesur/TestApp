using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Infrastructure.Data.PlayerProgress
{
    [Serializable]
    public class ProgressData
    {
        public int ActiveLevel;
        public int BestScore;
        public int SelectedItemId;
        public int ActiveBoosterId;
        public int DailyBonusStreak;
        public int DailyBonusLastClaimDay;
        public int LastShowDailyAdRewardDay;
        public int LastLuckySpinUseDay;
        public long ActiveBoosterEndUnixSeconds;
        public string UserId;
        public bool MuteSounds;
        public List<int> WinLevelIds = new();
        public List<int> UnlockedLevelItemIds = new();
        public List<int> PurchasedShopProductIds = new();
        public List<string> PendingInAppProducts = new();
        public List<ProgressResourceData> Resources = new();

        public void AddWinLevel(int levelId)
        {
            if (WinLevelIds.Contains(levelId))
                return;
            
            WinLevelIds.Add(levelId);
        }

        public void AddPurchasedShopProduct(int productId)
        {
            if (PurchasedShopProductIds.Contains(productId))
                return;
            
            PurchasedShopProductIds.Add(productId);
        }

        public void AddUnlockedLevelItem(int itemId)
        {
            if (UnlockedLevelItemIds.Contains(itemId))
                return;
            
            UnlockedLevelItemIds.Add(itemId);
        }

        public void MarkProductAsPending(string productId)
        {
            if (PendingInAppProducts.Contains(productId))
                return;
            
            PendingInAppProducts.Add(productId);
        }

        public void RemoveProductFromPending(string productId)
        {
            if (!PendingInAppProducts.Contains(productId))
                return;
            
            PendingInAppProducts.Remove(productId);
        }

        public void RemoveResource(int resourceId)
        {
            var index = Resources.FindIndex(r => r.resourceId == resourceId);
            if (index < 0) return;
            
            Resources.RemoveAt(index);
        }

        public void ChangeResourceAmount(int resourceId, int resourceAmount)
        {
           var resource = Resources.Find(resourceData => resourceData.resourceId == resourceId);
           if (resource == null)
           {
               resource = new ProgressResourceData
               {
                   resourceId = resourceId
               };
               Resources.Add(resource);
           }
           
           resource.amount = resourceAmount;
        }
    }
}