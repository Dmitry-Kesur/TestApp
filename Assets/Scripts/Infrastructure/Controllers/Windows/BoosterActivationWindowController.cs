using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.Booster;
using Infrastructure.Services.Level;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class BoosterActivationWindowController : BaseWindowController<BoosterActivationWindow>
    {
        private readonly IBoostersService _boostersService;
        private readonly LevelFlowService _levelFlowService;
        private readonly BoosterActivationWindowModel _windowModel;

        public BoosterActivationWindowController(IBoostersService boostersService, LevelFlowService levelFlowService)
        {
            _boostersService = boostersService;
            _levelFlowService = levelFlowService;
            _windowModel = new BoosterActivationWindowModel();
            _windowModel.CancelAction += OnCancel;
            _boostersService.OnBoosterActivatedAction += OnBoosterActivated;
        }

        protected override void InitParameters()
        {
            base.InitParameters();
            _windowModel.GetBoosterModels = _boostersService.GetAvailableBoosters();
        }

        protected override BaseWindowModel GetModel() => _windowModel;

        private void OnCancel() =>
            _levelFlowService.StartLevel();

        private void OnBoosterActivated() =>
            _levelFlowService.StartLevel();
    }
}