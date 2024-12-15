using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class AuthenticationWindow : BaseWindow
    {
        [SerializeField] private BaseButton _signInButton;

        private AuthenticationWindowModel _windowModel;
        
        public override void Init()
        {
            base.Init();
            _signInButton.OnButtonClickAction = OnSignInButtonClicked;
        }

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _windowModel = model as AuthenticationWindowModel;
        }

        public override Type GetWindowControllerType() =>
            typeof(AuthenticationWindowController);

        private void OnSignInButtonClicked() =>
            _windowModel.OnSignInButtonClicked();
    }
}