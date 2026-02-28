using Infrastructure.Enums;
using Infrastructure.Models.UI.HUD;
using Infrastructure.Services;
using Infrastructure.Services.Booster;
using Infrastructure.Services.Level;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.HUD;

namespace Infrastructure.Controllers.Hud
{
    public class HudController
    {
        private readonly HudModel _hudModel;
        private readonly ILevelsService _levelsService;
        private readonly IBoostersService _boostersService;
        private readonly StateMachineService _stateMachineService;

        private HudView _hudView;

        public HudController(StateMachineService stateMachineService, ILevelsService levelsService, IBoostersService boostersService)
        {
            _stateMachineService = stateMachineService;
            _levelsService = levelsService;
            _boostersService = boostersService;
            _boostersService.OnBoosterDeactivatedAction += OnBoosterDeactivated;

            _hudModel = new HudModel(_levelsService)
            {
                ActiveBoosterModel = boostersService.ActiveBooster,
                OnPauseGameButtonClickAction = OnPauseGameButtonClickHandler
            };
        }

        public void OnShowHud(HudView hudView)
        {
            _hudView = hudView;
            _hudView.SetModel(_hudModel);
            _hudView.Draw();
        }

        public void OnUpdate()
        {
            OnUpdateLevelProgress();
        }

        private void UpdateActiveBooster()
        {
            _hudView.UpdateActiveBooster();
        }

        private void OnUpdateLevelProgress()
        {
            _hudView.UpdateLevelProgress();
        }
        
        private void OnBoosterDeactivated()
        {
            _hudModel.ActiveBoosterModel = null;
            UpdateActiveBooster();
        }

        private void OnPauseGameButtonClickHandler()
        {
            _stateMachineService.TransitionTo(StateType.PauseGameLoopState);
        }
    }
}