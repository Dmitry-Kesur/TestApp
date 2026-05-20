using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Reward;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class MenuWindowController : BaseWindowController<MenuWindow>
    {
        private readonly MenuWindowModel _menuWindowModel;
        private readonly StateMachineService _stateMachineService;
        private readonly IAuthenticationService _authenticationService;
        private readonly DailyAdsService _dailyAdsService;

        public MenuWindowController(SaveLoadProgressService saveLoadProgressService,
            StateMachineService stateMachineService, IAuthenticationService authenticationService, DailyAdsService dailyAdsService)
        {
            _stateMachineService = stateMachineService;
            _authenticationService = authenticationService;
            _dailyAdsService = dailyAdsService;

            _menuWindowModel = new MenuWindowModel(_authenticationService)
            {
                OnPlayButtonClickAction = OnPlayButtonClick,
                OnSettingsButtonClickAction = OnSettingsButtonClick,
                OnShopButtonClickAction = OnShopButtonClick,
                OnBoostersButtonClickAction = OnBoostersButtonClick,
                OnDailyRewardButtonClickAction = OnDailyRewardButtonClick,
                OnLuckySpinButtonClickAction = OnLuckySpinButtonClick,
                BestScore = saveLoadProgressService.Read(progress => progress.BestScore)
            };
        }

        protected override BaseWindowModel GetModel() =>
            _menuWindowModel;

        private void OnSettingsButtonClick() =>
            _stateMachineService.TransitionTo(StateType.SettingsState);

        private void OnPlayButtonClick() =>
            _stateMachineService.TransitionTo(StateType.SelectLevelState);

        private void OnShopButtonClick() =>
            _stateMachineService.TransitionTo(StateType.ShopState);

        private void OnBoostersButtonClick() =>
            _stateMachineService.TransitionTo(StateType.BoostersState);

        private void OnDailyRewardButtonClick() =>
            _dailyAdsService.ShowRewardedAds();

        private void OnLuckySpinButtonClick() =>
            _stateMachineService.TransitionTo(StateType.LuckySpinState);
    }
}