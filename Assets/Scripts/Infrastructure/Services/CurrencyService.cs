using Infrastructure.Models.GameEntities.Currency;
using Infrastructure.Services.PlayerProgressUpdaters;

namespace Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ResourceProgressUpdater _resourceProgressUpdater;

        private CurrencyModel _currencyModel;

        public CurrencyService(ResourceProgressUpdater resourceProgressUpdater)
        {
            _resourceProgressUpdater = resourceProgressUpdater;
            _resourceProgressUpdater.OnLoadedResourcesProgress += CreateCurrencyModel;
        }

        public void IncreaseCurrency(int increaseAmount) =>
            _currencyModel.Increase(increaseAmount);

        public void DecreaseCurrency(int decreaseAmount) =>
            _currencyModel.Decrease(decreaseAmount);

        public int CurrencyAmount =>
            _currencyModel.CurrencyAmount;

        public CurrencyModel GetCurrencyModel() =>
            _currencyModel;

        private void OnUpdateCurrency() =>
            _resourceProgressUpdater.UpdateCurrencyAmount(CurrencyAmount);

        private void CreateCurrencyModel()
        {
            _currencyModel = new CurrencyModel();
            _currencyModel.UpdateCurrency(_resourceProgressUpdater.GetCurrencyAmount());
            _currencyModel.OnUpdateCurrencyAction += OnUpdateCurrency;
        }
    }
}