using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.Level;
using Infrastructure.Services.Sound;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class PauseGameWindowController : BaseWindowController<PauseGameWindow>
    {
        private readonly PauseGameWindowModel _pauseGameWindowModel;
        private readonly StateMachineService _stateMachineService;
        private readonly ILevelsService _levelsService;
        private readonly ISoundService _soundService;

        public PauseGameWindowController(StateMachineService stateMachineService, ILevelsService levelsService, ISoundService soundService)
        {
            _stateMachineService = stateMachineService;
            _levelsService = levelsService;
            _soundService = soundService;
            _soundService.OnChangeMuteSoundsAction += OnChangeMuteSounds;
            
            _pauseGameWindowModel = new PauseGameWindowModel
            {
                OnBackToMenuButtonClickAction = OnBackToMenuButtonClick,
                OnResumeGameButtonClickAction = OnResumeGameButtonClick,
                OnMuteSoundsStateChange = OnMuteSoundsStateChange,
            };
        }

        protected override BaseWindowModel GetModel() =>
            _pauseGameWindowModel;

        private void OnResumeGameButtonClick()
        {
            _stateMachineService.TransitionTo(StateType.GameLoopState);
        }

        private void OnBackToMenuButtonClick()
        {
            _levelsService.Stop();
            _stateMachineService.TransitionTo(StateType.MenuState);
        }

        private void OnMuteSoundsStateChange(bool muteSounds)
        {
            _soundService.ChangeMuteSounds(muteSounds);
        }

        private void OnChangeMuteSounds(bool muteSounds) =>
            _pauseGameWindowModel.MuteSounds = muteSounds;
    }
}