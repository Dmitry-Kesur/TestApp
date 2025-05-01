using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using Infrastructure.Views.UI.Loaders;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class BoostersWindow : BaseWindow
    {
        [SerializeField] private BoostersLoader _boostersLoader;
        [SerializeField] private ButtonWithLabel _backToMenuButton;

        private BoostersWindowModel _boostersWindowModel;

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _boostersWindowModel = model as BoostersWindowModel;
        }

        public override Type GetWindowControllerType() =>
            typeof(BoostersWindowController);

        protected override void Draw()
        {
            base.Draw();
            _boostersLoader.DrawLoader(_boostersWindowModel.GetBoosters());
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
            _boostersWindowModel.OnBackToMenuButtonClicked();
    }
}