using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using Infrastructure.Views.UI.Loaders;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class PremiumShopWindow : BaseWindow
    {
        [SerializeField] private PremiumShopLoader premiumShopLoader;
        [SerializeField] private ButtonWithLabel _backToMenuButton;

        private PremiumShopWindowModel _premiumShopWindowModel;

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _premiumShopWindowModel = model as PremiumShopWindowModel;
        }

        public override Type GetWindowControllerType() =>
            typeof(PremiumShopWindowController);

        protected override void Draw()
        {
            base.Draw();
            premiumShopLoader.DrawLoader(_premiumShopWindowModel.GetProductModels());
        }

        protected override void SubscribeListeners()
        {
            base.SubscribeListeners();
            _backToMenuButton.OnButtonClickAction = OnBackToMenuButtonClicked;
        }

        protected override void Clear()
        {
            base.Clear();
            _backToMenuButton.OnButtonClickAction = null;
        }

        private void OnBackToMenuButtonClicked() =>
            _premiumShopWindowModel.OnBackToMenuButtonClicked();
    }
}