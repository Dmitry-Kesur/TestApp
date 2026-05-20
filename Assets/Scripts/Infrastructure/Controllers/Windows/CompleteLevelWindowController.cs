using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.Level;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class CompleteLevelWindowController : BaseWindowController<WinLevelWindow>
    {
        private readonly ILevelsService _levelsService;
        private readonly LevelFlowService _levelFlowService;

        private CompleteLevelWindowModel _completeLevelWindowModel;

        public CompleteLevelWindowController(ILevelsService levelsService, LevelFlowService levelFlowService)
        {
            _levelsService = levelsService;
            _levelFlowService = levelFlowService;

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

        private void OnMenuButtonClick() => _levelFlowService.BackToMenu();

        private void OnNextLevelButtonClick() => _levelFlowService.StartLevel();
    }
}