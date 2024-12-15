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

        public override void Init()
        {
            base.Init();
            _backToMenuButton.OnButtonClickAction = OnBackToMenuButtonClicked;
            
            _boostersLoader.DrawLoader(_boostersWindowModel.GetBoosters());
        }
        
        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _boostersWindowModel = model as BoostersWindowModel;
        }

        public override Type GetWindowControllerType() =>
            typeof(BoostersWindowController);

        private void OnBackToMenuButtonClicked() =>
            _boostersWindowModel.OnBackToMenuButtonClicked();
    }
}