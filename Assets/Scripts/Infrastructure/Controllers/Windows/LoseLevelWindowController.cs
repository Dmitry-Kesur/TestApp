using Infrastructure.Constants;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Level;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class LoseLevelWindowController : BaseWindowController<LoseLevelWindow>
    {
        private readonly LoseLevelWindowModel _loseLevelWindowModel;
        private readonly LevelFlowService _levelFlowService;
        private readonly IAdsService _adsService;

        public LoseLevelWindowController(LevelFlowService levelFlowService, ILevelsService levelsService,
            IAdsService adsService)
        {
            _levelFlowService = levelFlowService;
            _adsService = adsService;

            _loseLevelWindowModel = new LoseLevelWindowModel(levelsService.GetCurrentLevel())
            {
                OnRestartButtonClickAction = OnRestartButtonClick,
                OnBackToMenuButtonClickAction = OnBackToMenuButtonClick,
                OnContinueButtonClickAction = OnContinueButtonClick
            };
        }

        protected override BaseWindowModel GetModel() =>
            _loseLevelWindowModel;

        private void OnBackToMenuButtonClick() => _levelFlowService.BackToMenu();

        private void OnRestartButtonClick() => _levelFlowService.RestartLevel();

        private void OnContinueButtonClick() => _adsService.ShowAds(AdsId.Rewarded, completeCallback: _levelFlowService.ReviveLevel);
    }
}