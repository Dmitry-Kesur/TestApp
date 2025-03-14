using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Providers;
using Infrastructure.Services;
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
            _stateMachineService = stateMachineService;

            _selectLevelWindowModel = new SelectLevelWindowModel
            {
                OnBackButtonClickAction = OnBackToMenu,
                OnLevelSelectAction = OnLevelSelect
            };
        }

        public override void OnWindowAdd(BaseWindow view)
        {
            base.OnWindowAdd(view);
            windowView.SetModel(_selectLevelWindowModel);
            _selectLevelWindowModel.SetLevelPreviews(_levelsService.GetPreviewsModels());
        }

        private void OnLevelSelect(int level)
        {
            _levelsService.Select(level);
            _stateMachineService.TransitionTo(StateType.GameLoopState);
        }

        private void OnBackToMenu() =>
            _stateMachineService.TransitionTo(StateType.MenuState);
    }
}