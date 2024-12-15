using System;
using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Currency;
using Infrastructure.Models.GameEntities.Products;
using Infrastructure.Models.GameEntities.Products.InGame;

namespace Infrastructure.Models.UI.Windows
{
    public class ShopWindowModel : BaseWindowModel
    {
        public Action OnBackButtonClickAction;

        private List<ProductModel> _shopProducts;
        private CurrencyModel _currencyModel;

        public void OnBackButtonClick() =>
            OnBackButtonClickAction?.Invoke();

        public void SetProducts(List<ProductModel> shopProducts) =>
            _shopProducts = shopProducts;

        public void SetCurrencyModel(CurrencyModel currencyModel) =>
            _currencyModel = currencyModel;

        public List<ProductModel> ShopProducts => 
            _shopProducts;

        public CurrencyModel CurrencyModel =>
            _currencyModel;
    }
}