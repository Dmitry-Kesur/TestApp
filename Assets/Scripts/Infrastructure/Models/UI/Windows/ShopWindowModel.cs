using System;
using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Products.InGame;
using Infrastructure.Services;
using Infrastructure.Services.Currency;

namespace Infrastructure.Models.UI.Windows
{
    public class ShopWindowModel : BaseWindowModel
    {
        public Action OnBackButtonClickAction;
        public Action<int> OnUpdateCurrencyAction;
        
        private readonly ICurrencyService _currencyService;

        private List<ProductModel> _shopProducts;

        public ShopWindowModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            _currencyService.OnUpdateCurrencyAction = OnUpdateCurrency;
        }

        public void OnBackButtonClick() =>
            OnBackButtonClickAction?.Invoke();

        public void SetProducts(List<ProductModel> shopProducts) =>
            _shopProducts = shopProducts;

        public List<ProductModel> ShopProducts => 
            _shopProducts;

        private void OnUpdateCurrency() =>
            OnUpdateCurrencyAction?.Invoke(_currencyService.CurrencyAmount);
    }
}