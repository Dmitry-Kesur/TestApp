using System;
using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.PlayerProgressUpdaters
{
    public class ResourceProgressUpdater : IProgressUpdater
    {
        public Action OnLoadedResourcesProgress;
        
        public void OnLoadProgress(PlayerProgress playerProgress)
        {
            Progress = playerProgress;
            OnLoadedResourcesProgress?.Invoke();
        }
        
        public PlayerProgress Progress { get; set; }

        public int GetActiveBoosterId() =>
            Progress.ActiveBoosterId;

        public int GetCurrencyAmount() =>
            Progress.CurrencyAmount;

        public List<int> GetPurchasedProductIds() =>
            Progress.PurchasedProductIds;

        public void SetActiveBoosterId(int boosterId)
        {
            Progress.ActiveBoosterId = boosterId;
        }

        public void UpdateCurrencyAmount(int currencyAmount)
        {
            Progress.CurrencyAmount = currencyAmount;
        }

        public void SetPurchasedProductId(int productId)
        {
            Progress.PurchasedProductIds.Add(productId);
        }
    }
}