using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.DailyBonus;
using Infrastructure.Services.Preloader;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class PreloaderWindowController : BaseWindowController<PreloaderWindow>
    {
        private readonly IPreloaderService _preloaderService;
        private readonly PreloaderWindowModel _preloaderWindowModel;
        private readonly StateMachineService _stateMachineService;
        private readonly DailyBonusService _dailyBonusService;

        public PreloaderWindowController(IPreloaderService preloaderService, StateMachineService stateMachineService, DailyBonusService dailyBonusService)
        {
            _preloaderService = preloaderService;
            _preloaderService.UpdateLoadingProgressAction = OnUpdateLoadingProgress;

            _stateMachineService = stateMachineService;
            _dailyBonusService = dailyBonusService;

            _preloaderWindowModel = new PreloaderWindowModel();
            _preloaderWindowModel.StartGameAction = OnStartGame;
        }

        protected override BaseWindowModel GetModel() =>
            _preloaderWindowModel;

        private void OnStartGame()
        {
            var nextState = _dailyBonusService.CanTakeReward ? StateType.DailyBonusState : StateType.MenuState;
            _stateMachineService.TransitionTo(nextState);
        }

        private void OnUpdateLoadingProgress(float progress, string stageText)
        {
            _preloaderWindowModel.OnUpdateLoadingProgress(progress, stageText);
        }
    }
}