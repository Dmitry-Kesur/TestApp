using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class PauseGameWindow : BaseWindow
    {
        [SerializeField] private ButtonWithLabel _backToMenuButton;
        [SerializeField] private ButtonWithLabel _resumeGameButton;
        [SerializeField] private ToggleButton _muteSoundsToggle;
        
        private PauseGameWindowModel _pauseGameWindowModel;

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _pauseGameWindowModel = windowModel as PauseGameWindowModel;
        }

        public override Type GetWindowControllerType() => typeof(PauseGameWindowController);

        protected override void SubscribeListeners()
        {
            base.SubscribeListeners();
            _backToMenuButton.OnButtonClickAction = OnBackToMenuButtonClick;
            _resumeGameButton.OnButtonClickAction = OnResumeGameButtonClick;
            _muteSoundsToggle.OnToggleStateChange = OnChangeMuteSoundsState;
        }

        protected override void Draw()
        {
            base.Draw();
            _muteSoundsToggle.ChangeToggleState(_pauseGameWindowModel.MuteSounds);
        }

        protected override void Clear()
        {
            base.Clear();
            _backToMenuButton.OnButtonClickAction = null;
            _resumeGameButton.OnButtonClickAction = null;
            _muteSoundsToggle.OnToggleStateChange = null;
        }

        private void OnChangeMuteSoundsState(bool toggleState)
        {
            _pauseGameWindowModel.ChangeMuteSoundsState(toggleState);
        }

        private void OnBackToMenuButtonClick()
        {
            _pauseGameWindowModel.OnBackToMenuButtonClick();
        }

        private void OnResumeGameButtonClick()
        {
            _pauseGameWindowModel.OnResumeGameButtonClick();
        }
    }
}