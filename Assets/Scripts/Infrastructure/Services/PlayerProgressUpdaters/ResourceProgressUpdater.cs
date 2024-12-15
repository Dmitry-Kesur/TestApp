using System;
using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.PlayerProgressUpdaters
{
    public class ResourceProgressUpdater : IProgressUpdater
    {
        private int _activeBoosterId;
        private int _currencyAmount;
        private List<int> _purchasedProductIds;

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.ActiveBoosterId = _activeBoosterId;
            playerProgress.CurrencyAmount = _currencyAmount;
            playerProgress.PurchasedProductIds = _purchasedProductIds;
        }

        public void OnLoadProgress(PlayerProgress playerProgress)
        {
            _activeBoosterId = playerProgress.ActiveBoosterId;
            _currencyAmount = playerProgress.CurrencyAmount;
            _purchasedProductIds = playerProgress.PurchasedProductIds;
        }

        public Action OnProgressChanged { get; set; }

        public int GetActiveBoosterId() =>
            _activeBoosterId;

        public int GetCurrencyAmount() =>
            _currencyAmount;

        public List<int> GetPurchasedProductIds() =>
            _purchasedProductIds;

        public void SetActiveBoosterId(int boosterId)
        {
            _activeBoosterId = boosterId;
            OnProgressChanged?.Invoke();
        }

        public void UpdateCurrencyAmount(int currencyAmount)
        {
            _currencyAmount = currencyAmount;
            OnProgressChanged?.Invoke();
        }

        public void SetPurchasedProductId(int productId)
        {
            _purchasedProductIds.Add(productId);
            OnProgressChanged?.Invoke();
        }
    }
}