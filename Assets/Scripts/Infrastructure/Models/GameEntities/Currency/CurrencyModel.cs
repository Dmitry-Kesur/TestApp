using System;

namespace Infrastructure.Models.GameEntities.Currency
{
    public class CurrencyModel
    {
        public Action OnUpdateCurrencyAction;

        private int _currencyAmount;

        public void UpdateCurrency(int currencyAmount)
        {
            _currencyAmount = currencyAmount;
            OnUpdateCurrencyAction?.Invoke();
        }

        public void Increase(int increaseAmount)
        {
            var increasedAmount = _currencyAmount + increaseAmount;
            UpdateCurrency(increasedAmount);
        }

        public void Decrease(int decreaseAmount)
        {
            var decreasedAmount = _currencyAmount - decreaseAmount;
            UpdateCurrency(decreasedAmount);
        }

        public int CurrencyAmount =>
            _currencyAmount;
    }
}