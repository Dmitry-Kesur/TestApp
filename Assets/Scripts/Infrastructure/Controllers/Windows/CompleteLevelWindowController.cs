using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.Level;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class CompleteLevelWindowController : BaseWindowController<WinLevelWindow>
    {
        private readonly ILevelsService _levelsService;
        private readonly StateMachineService _stateMachineService;

        private CompleteLevelWindowModel _completeLevelWindowModel;

        public CompleteLevelWindowController(ILevelsService levelsService,
            StateMachineService stateMachineService)
        {
            _levelsService = levelsService;
            _stateMachineService = stateMachineService;

            CreateWindowModel();
        }

        protected override void InitParameters()
        {
            base.InitParameters();
            var levelResult = _levelsService.LevelResult;
            _completeLevelWindowModel.LevelScore = levelResult.Score;
            _completeLevelWindowModel.CanStartNextLevel = !_levelsService.ReachedMaxLevel;
        }

        protected override BaseWindowModel GetModel() =>
            _completeLevelWindowModel;

        private void CreateWindowModel()
        {
            _completeLevelWindowModel = new CompleteLevelWindowModel
            {
                OnNextLevelButtonClickAction = OnNextLevelButtonClick,
                OnMenuButtonClickAction = OnMenuButtonClick
            };
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