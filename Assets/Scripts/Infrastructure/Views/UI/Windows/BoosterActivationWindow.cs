using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using Infrastructure.Views.UI.Loaders;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class BoosterActivationWindow : BaseWindow
    {
        [SerializeField] private BoostersLoader _boostersLoader;
        [SerializeField] private ButtonWithLabel _cancelButton;

        private BoosterActivationWindowModel _boosterActivationWindowModel;

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _boosterActivationWindowModel = model as BoosterActivationWindowModel;
        }

        public override Type GetWindowControllerType() => typeof(BoosterActivationWindowController);

        protected override void Draw()
        {
            base.Draw();
            _boostersLoader.DrawLoader(_boosterActivationWindowModel.GetBoosterModels);
        }

        protected override void SubscribeListeners()
        {
            base.SubscribeListeners();
            _cancelButton.OnButtonClickAction += OnCancelButtonClicked;
        }

        protected override void Clear()
        {
            base.Clear();
            _cancelButton.OnButtonClickAction -= OnCancelButtonClicked;
        }
        
        private void OnCancelButtonClicked() =>
            _boosterActivationWindowModel.OnCancelButtonClicked();
    }
}