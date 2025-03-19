using System;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;

namespace Infrastructure.Services.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ResourceProgressUpdater _resourceProgressUpdater;

        public CurrencyService(ResourceProgressUpdater resourceProgressUpdater)
        {
            _resourceProgressUpdater = resourceProgressUpdater;
        }

        public Action OnUpdateCurrencyAction { get; set; }

        public void IncreaseCurrency(int increaseAmount)
        {
            var newCurrencyAmount = CurrencyAmount + increaseAmount;
            _resourceProgressUpdater.UpdateCurrencyAmount(newCurrencyAmount);
            
            OnUpdateCurrencyAction?.Invoke();
        }

        public void DecreaseCurrency(int decreaseAmount)
        {
            var newCurrencyAmount = CurrencyAmount - decreaseAmount;
            _resourceProgressUpdater.UpdateCurrencyAmount(newCurrencyAmount);
            
            OnUpdateCurrencyAction?.Invoke();
        }

        public int CurrencyAmount =>
            _resourceProgressUpdater.GetCurrencyAmount();
    }
}