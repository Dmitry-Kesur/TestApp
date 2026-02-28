using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using Infrastructure.Views.UI.Loaders;
using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure.Views.UI.Windows
{
    public class ShopWindow : BaseWindow
    {
        [SerializeField] private ButtonWithLabel _backButton;
        [SerializeField] private ShopLoader _shopLoader;
        [SerializeField] private CoinsView _coinsView;

        private ShopWindowModel _shopWindowModel;

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _shopWindowModel = model as ShopWindowModel;
        }

        public override Type GetWindowControllerType() => 
            typeof(ShopWindowController);

        protected override void SubscribeListeners()
        {
            base.SubscribeListeners();
            _backButton.OnButtonClickAction += _shopWindowModel.OnBackButtonClick;
            _shopWindowModel.OnUpdateCoinsAction += UpdateCoins;
            _shopWindowModel.OnCompletePurchaseAction += DrawProducts;
        }

        protected override void Draw()
        {
            base.Draw();
            DrawProducts();
            UpdateCoins();
        }

        private void DrawProducts()
        {
            _shopLoader.DrawLoader(_shopWindowModel.ShopProducts);
        }

        protected override void Clear()
        {
            base.Clear();

            _backButton.OnButtonClickAction -= _shopWindowModel.OnBackButtonClick;
            _shopWindowModel.OnUpdateCoinsAction -= UpdateCoins;
            _shopWindowModel.OnCompletePurchaseAction -= DrawProducts;
        }

        private void UpdateCoins()
        {
            _coinsView.UpdateCoins(_shopWindowModel.CoinsAmount);
        }
    }
}