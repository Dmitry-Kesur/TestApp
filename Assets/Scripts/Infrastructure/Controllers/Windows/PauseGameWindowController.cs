using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.Level;
using Infrastructure.Services.Sound;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class PauseGameWindowController : BaseWindowController<PauseGameWindow>
    {
        private readonly PauseGameWindowModel _pauseGameWindowModel;
        private readonly LevelFlowService _levelFlowService;
        private readonly ISoundService _soundService;

        public PauseGameWindowController(LevelFlowService levelFlowService,
            ISoundService soundService)
        {
            _levelFlowService = levelFlowService;
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

        private void OnResumeGameButtonClick() => _levelFlowService.ResumeLevel();

        private void OnBackToMenuButtonClick() => _levelFlowService.BackToMenu();

        private void OnMuteSoundsStateChange(bool muteSounds)
        {
            _soundService.ChangeMuteSounds(muteSounds);
        }

        private void OnChangeMuteSounds(bool muteSounds) =>
            _pauseGameWindowModel.MuteSounds = muteSounds;
    }
}