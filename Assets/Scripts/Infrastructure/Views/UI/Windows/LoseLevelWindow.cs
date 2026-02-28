using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class LoseLevelWindow : BaseWindow
    {
        [SerializeField] private ButtonWithLabel _restartButton;
        [SerializeField] private ButtonWithLabel _backToMenuButton;
        [SerializeField] private ButtonWithLabel _continueButton;
        [SerializeField] private TextMeshProUGUI _totalScoreTextField;
        
        private LoseLevelWindowModel _loseLevelWindowModel;

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _loseLevelWindowModel = model as LoseLevelWindowModel;
        }

        public override Type GetWindowControllerType() => typeof(LoseLevelWindowController);

        protected override void Draw()
        {
            base.Draw();
            _totalScoreTextField.text = _loseLevelWindowModel.TotalScore.ToString();
        }

        protected override void SubscribeListeners()
        {
            base.SubscribeListeners();
            _restartButton.OnButtonClickAction = _loseLevelWindowModel.OnRestartButtonClick;
            _backToMenuButton.OnButtonClickAction = _loseLevelWindowModel.OnBackToMenuButtonClick;
            _continueButton.OnButtonClickAction = _loseLevelWindowModel.OnContinueButtonClick;
        }

        protected override void Clear()
        {
            base.Clear();
            _restartButton.OnButtonClickAction = null;
            _backToMenuButton.OnButtonClickAction = null;
        }
    }
}