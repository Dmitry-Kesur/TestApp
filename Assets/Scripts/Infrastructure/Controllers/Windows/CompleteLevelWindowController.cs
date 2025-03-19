using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.Level;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class CompleteLevelWindowController : BaseWindowController<WinLevelWindow>
    {
        private readonly CompleteLevelWindowModel _completeLevelWindowModel;
        private readonly ILevelsService _levelsService;
        private readonly StateMachineService _stateMachineService;

        public CompleteLevelWindowController(ILevelsService levelsService,
            StateMachineService stateMachineService)
        {
            _levelsService = levelsService;
            _stateMachineService = stateMachineService;

            _completeLevelWindowModel = new CompleteLevelWindowModel
            {
                OnNextLevelButtonClickAction = OnNextLevelButtonClick,
                OnMenuButtonClickAction = OnMenuButtonClick
            };
        }

        public override void OnWindowAdd(BaseWindow view)
        {
            base.OnWindowAdd(view);
            _completeLevelWindowModel.SetWinLevel(_levelsService.GetCurrentLevel());
            _completeLevelWindowModel.CanStartNextLevel = !_levelsService.ReachedMaxLevel;
            windowView.SetModel(_completeLevelWindowModel);
        }

        private void OnMenuButtonClick()
        {
            _stateMachineService.TransitionTo(StateType.MenuState);
        }

        private void OnNextLevelButtonClick()
        {
            _stateMachineService.TransitionTo(StateType.GameLoopState);
        }
    }
}