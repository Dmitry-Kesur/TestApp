using Infrastructure.Constants;
using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Level;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class LoseLevelWindowController : BaseWindowController<LoseLevelWindow>
    {
        private readonly ILevelsService _levelsService;
        private readonly LoseLevelWindowModel _loseLevelWindowModel;
        private readonly StateMachineService _stateMachineService;
        private readonly IAdsService _adsService;

        public LoseLevelWindowController(ILevelsService levelsService,
            StateMachineService stateMachineService, IAdsService adsService)
        {
            _levelsService = levelsService;
            _stateMachineService = stateMachineService;
            _adsService = adsService;

            _loseLevelWindowModel = new LoseLevelWindowModel(_levelsService.GetCurrentLevel())
            {
                OnRestartButtonClickAction = OnRestartButtonClick,
                OnBackToMenuButtonClickAction = OnBackToMenuButtonClick,
                OnContinueButtonClickAction = OnContinueButtonClick
            };
        }

        protected override BaseWindowModel GetModel() =>
            _loseLevelWindowModel;

        private void OnBackToMenuButtonClick() =>
            _stateMachineService.TransitionTo(StateType.MenuState);

        private void OnRestartButtonClick() =>
            _stateMachineService.TransitionTo(StateType.GameLoopState);

        private void OnContinueButtonClick()
        {
            _adsService.OnAdsShowCompletedAction += OnAdsShowCompleted;
            _adsService.ShowAds(AdsId.Rewarded);
        }

        private void OnAdsShowCompleted(string adsId)
        {
            if (adsId != AdsId.Rewarded)
                return;
            
            _adsService.OnAdsShowCompletedAction -= OnAdsShowCompleted;
            _levelsService.Revive();
            _stateMachineService.TransitionTo(StateType.GameLoopState);
        }
    }
}