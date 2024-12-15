using System.Threading.Tasks;
using Infrastructure.Controllers.Hud;
using Infrastructure.Providers;
using Infrastructure.Views.UI.HUD;
using Zenject;

namespace Infrastructure.Services
{
    public class HudService : IHudService
    {
        private readonly LocalAddressableService _addressableService;
        private readonly UIProvider _uiProvider;
        private readonly DiContainer _diContainer;

        private HudView _hudView;
        private HudController _hudController;

        public HudService(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _addressableService = _diContainer.Resolve<LocalAddressableService>();
            _uiProvider = _diContainer.Resolve<UIProvider>();
        }

        public async Task ShowHud()
        {
            _hudView = await _addressableService.InstantiatePrefab<HudView>("HudView");
            _hudView.transform.SetParent(_uiProvider.HudLayer, false);

            _hudController ??= GetHudController();
            _hudController.OnShowHud(_hudView);
        }

        public void HideHud()
        {
            _hudView.Clear();
            _addressableService.Release(_hudView.gameObject);
            _hudView = null;
        }

        public void UpdateHud() =>
            _hudController.OnUpdate();
        
        private HudController GetHudController()
        {
            var hudController = _diContainer.Instantiate<HudController>();
            return hudController;
        }
    }
}