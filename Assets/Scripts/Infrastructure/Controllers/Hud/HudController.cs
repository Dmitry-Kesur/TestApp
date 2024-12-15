using Infrastructure.Enums;
using Infrastructure.Models.UI.HUD;
using Infrastructure.Services;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.HUD;

namespace Infrastructure.Controllers.Hud
{
    public class HudController
    {
        private readonly HudModel _hudModel;
        private readonly ILevelsService _levelsService;
        private readonly StateMachineService _stateMachineService;

        private HudView _hudView;

        public HudController(StateMachineService stateMachineService, ILevelsService levelsService, IBoostersService boostersService)
        {
            _stateMachineService = stateMachineService;
            _levelsService = levelsService;

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
            UpdateActiveBooster();
        }

        private void UpdateActiveBooster()
        {
            _hudView.UpdateActiveBooster();
        }

        private void OnUpdateLevelProgress()
        {
            _hudView.UpdateLevelProgress();
        }

        private void OnPauseGameButtonClickHandler()
        {
            _stateMachineService.TransitionTo(StateType.PauseGameLoopState);
        }
    }
}