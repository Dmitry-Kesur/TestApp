using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Providers.Level;
using Infrastructure.Services.Booster;
using Infrastructure.Services.Level;
using Infrastructure.Services.Window;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class SelectLevelWindowController : BaseWindowController<SelectLevelWindow>
    {
        private readonly ILevelsService _levelsService;
        private readonly IBoostersService _boostersService;
        private readonly IWindowService _windowService;
        private readonly LevelsStaticDataProvider _levelsStaticDataProvider;
        private readonly StateMachineService _stateMachineService;
        private readonly SelectLevelWindowModel _selectLevelWindowModel;

        public SelectLevelWindowController(StateMachineService stateMachineService, ILevelsService levelsService, IBoostersService boostersService, IWindowService windowService)
        {
            _levelsService = levelsService;
            _boostersService = boostersService;
            _windowService = windowService;
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
            _levelsService.SelectLevel(level);
            
            if (_boostersService.HasBoosterToActivate)
            {
                _windowService.ShowWindow(WindowId.BoosterActivationWindow);
                return;
            }
            
            _stateMachineService.TransitionTo(StateType.GameLoopState);
        }

        private void OnBackToMenu() =>
            _stateMachineService.TransitionTo(StateType.MenuState);

        private void UpdateLevelPreviews() =>
            _selectLevelWindowModel.SetLevelPreviews(_levelsService.GetPreviewsModels());
    }
}