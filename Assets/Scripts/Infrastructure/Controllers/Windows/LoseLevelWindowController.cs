using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
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

        public LoseLevelWindowController(ILevelsService levelsService,
            StateMachineService stateMachineService)
        {
            _levelsService = levelsService;
            _stateMachineService = stateMachineService;

            _loseLevelWindowModel = new LoseLevelWindowModel(_levelsService.GetCurrentLevel())
            {
                OnRestartButtonClickAction = OnRestartButtonClick,
                OnBackToMenuButtonClickAction = OnBackToMenuButtonClickAction
            };
        }

        protected override BaseWindowModel GetModel() =>
            _loseLevelWindowModel;

        private void OnBackToMenuButtonClickAction() =>
            _stateMachineService.TransitionTo(StateType.MenuState);

        private void OnRestartButtonClick() =>
            _stateMachineService.TransitionTo(StateType.GameLoopState);
    }
}