using System;
using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Resources;
using Infrastructure.Models.GameEntities.Shop;

namespace Infrastructure.Models.UI.Windows
{
    public class ShopWindowModel : BaseWindowModel
    {
        public Action OnBackButtonClickAction;
        public Action OnUpdateCoinsAction;
        public Action OnCompletePurchaseAction;

        private IReadOnlyList<ShopProductModel> _shopProducts;

        private ResourceModel _coinResource;

        public ShopWindowModel(ResourceModel coinResource)
        {
            _coinResource = coinResource;
            _coinResource.OnAmountChange += OnUpdateCoins;
        }

        public int CoinsAmount => _coinResource.Amount;

        public void OnBackButtonClick() =>
            OnBackButtonClickAction?.Invoke();

        public void SetProducts(IReadOnlyList<ShopProductModel> shopProducts) =>
            _shopProducts = shopProducts;

        public IReadOnlyList<ShopProductModel> ShopProducts =>
            _shopProducts;

        public void OnCompletePurchase() =>
            OnCompletePurchaseAction?.Invoke();

        private void OnUpdateCoins() =>
            OnUpdateCoinsAction?.Invoke();
    }
}