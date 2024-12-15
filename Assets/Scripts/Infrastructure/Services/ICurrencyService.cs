using Infrastructure.Models.GameEntities.Currency;

namespace Infrastructure.Services
{
    public interface ICurrencyService
    {
        int CurrencyAmount { get; }
        CurrencyModel GetCurrencyModel();
        void IncreaseCurrency(int increaseAmount);
        void DecreaseCurrency(int decreaseAmount);
    }
}