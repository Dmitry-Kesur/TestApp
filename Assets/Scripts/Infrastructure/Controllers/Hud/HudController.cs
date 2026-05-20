using Infrastructure.Models.UI.HUD;
using Infrastructure.Services.Booster;
using Infrastructure.Services.Level;
using Infrastructure.Views.UI.HUD;

namespace Infrastructure.Controllers.Hud
{
    public class HudController
    {
        private readonly HudModel _hudModel;
        private readonly ILevelsService _levelsService;
        private readonly IBoostersService _boostersService;
        private readonly LevelFlowService _levelFlowService;

        private HudView _hudView;

        public HudController(ILevelsService levelsService, IBoostersService boostersService,
            LevelFlowService levelFlowService)
        {
            _levelsService = levelsService;
            _boostersService = boostersService;
            _levelFlowService = levelFlowService;
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

        private void OnPauseGameButtonClickHandler() => _levelFlowService.PauseLevel();
    }
}