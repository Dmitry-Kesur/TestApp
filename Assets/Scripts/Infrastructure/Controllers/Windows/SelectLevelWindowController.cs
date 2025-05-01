using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Providers;
using Infrastructure.Providers.Level;
using Infrastructure.Services;
using Infrastructure.Services.Level;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class SelectLevelWindowController : BaseWindowController<SelectLevelWindow>
    {
        private readonly ILevelsService _levelsService;
        private readonly LevelsStaticDataProvider _levelsStaticDataProvider;
        private readonly StateMachineService _stateMachineService;
        private readonly SelectLevelWindowModel _selectLevelWindowModel;

        public SelectLevelWindowController(StateMachineService stateMachineService, ILevelsService levelsService)
        {
            _levelsService = levelsService;
            _levelsService.OnWinLevelAction += OnWinLevel;
            _stateMachineService = stateMachineService;

            _selectLevelWindowModel = new SelectLevelWindowModel
            {
                OnBackButtonClickAction = OnBackToMenu,
                OnLevelSelectAction = OnLevelSelect
            };
            
            UpdateLevelPreviews();
        }

        protected override BaseWindowModel GetModel() =>
            _selectLevelWindowModel;

        private void OnLevelSelect(int level)
        {
            _levelsService.SetCurrentLevel(level);
            _stateMachineService.TransitionTo(StateType.GameLoopState);
        }

        private void OnBackToMenu() =>
            _stateMachineService.TransitionTo(StateType.MenuState);

        private void OnWinLevel() =>
            UpdateLevelPreviews();

        private void UpdateLevelPreviews() =>
            _selectLevelWindowModel.SetLevelPreviews(_levelsService.GetPreviewsModels());
    }
}