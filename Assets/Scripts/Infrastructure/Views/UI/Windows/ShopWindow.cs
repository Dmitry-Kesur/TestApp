﻿using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using Infrastructure.Views.UI.Loaders;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class ShopWindow : BaseWindow
    {
        [SerializeField] private ButtonWithLabel _backButton;
        [SerializeField] private ShopLoader _shopLoader;
        [SerializeField] private CurrencyView _currencyView;

        private ShopWindowModel _shopWindowModel;

        public override void Init()
        {
            base.Init();
            _backButton.OnButtonClickAction = _shopWindowModel.OnBackButtonClick;
            _shopLoader.DrawLoader(_shopWindowModel.ShopProducts);
            _currencyView.Init(_shopWindowModel.CurrencyModel);
        }

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _shopWindowModel = model as ShopWindowModel;
        }

        public override Type GetWindowControllerType() => 
            typeof(ShopWindowController);

        protected override void Clear()
        {
            base.Clear();
            _currencyView.Clear();
            _backButton.OnButtonClickAction = null;
        }
    }
}