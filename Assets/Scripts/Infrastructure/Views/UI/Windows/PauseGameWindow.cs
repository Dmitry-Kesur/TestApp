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

        public override void Init()
        {
            base.Init();
            _backToMenuButton.OnButtonClickAction = OnBackToMenuButtonClick;
            _resumeGameButton.OnButtonClickAction = OnResumeGameButtonClick;
            _muteSoundsToggle.OnToggleStateChange = OnChangeMuteSoundsState;
            
            _muteSoundsToggle.ChangeToggleState(_pauseGameWindowModel.MuteSounds);
        }

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _pauseGameWindowModel = windowModel as PauseGameWindowModel;
        }

        public override Type GetWindowControllerType() => typeof(PauseGameWindowController);

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