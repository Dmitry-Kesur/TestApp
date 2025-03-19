using System;

namespace Infrastructure.Services.Currency
{
    public interface ICurrencyService
    {
        int CurrencyAmount { get; }
        Action OnUpdateCurrencyAction { get; set; }
        void IncreaseCurrency(int increaseAmount);
        void DecreaseCurrency(int decreaseAmount);
    }
}