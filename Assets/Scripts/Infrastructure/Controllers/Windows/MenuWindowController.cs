using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class MenuWindowController : BaseWindowController<MenuWindow>
    {
        private readonly MenuWindowModel _menuWindowModel;
        private readonly StateMachineService _stateMachineService;
        private readonly IAuthenticationService _authenticationService;

        public MenuWindowController(LevelProgressUpdater levelProgressUpdater,
            StateMachineService stateMachineService, IAuthenticationService authenticationService)
        {
            _stateMachineService = stateMachineService;
            _authenticationService = authenticationService;

            _menuWindowModel = new MenuWindowModel(_authenticationService)
            {
                OnPlayButtonClickAction = OnPlayButtonClick,
                OnSettingsButtonClickAction = OnSettingsButtonClick,
                OnShopButtonClickAction = OnShopButtonClick,
                OnBoostersButtonClickAction = OnBoostersButtonClick
            };

            _menuWindowModel.BestScore = levelProgressUpdater.GetBestScore();
        }

        public override void OnWindowAdd(BaseWindow view)
        {
            base.OnWindowAdd(view);
            windowView.SetModel(_menuWindowModel);
        }

        private void OnSettingsButtonClick() =>
            _stateMachineService.TransitionTo(StateType.SettingsState);

        private void OnPlayButtonClick() =>
            _stateMachineService.TransitionTo(StateType.SelectLevelState);

        private void OnShopButtonClick() =>
            _stateMachineService.TransitionTo(StateType.ShopState);

        private void OnBoostersButtonClick() =>
            _stateMachineService.TransitionTo(StateType.BoostersState);
    }
}