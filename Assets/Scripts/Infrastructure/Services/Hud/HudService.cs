using System.Threading.Tasks;
using Infrastructure.Controllers.Hud;
using Infrastructure.Providers.UI;
using Infrastructure.Services.Addressable;
using Infrastructure.Views.UI.HUD;
using Zenject;

namespace Infrastructure.Services.Hud
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
            if (_hudView == null)
            {
                _hudView = await _addressableService.InstantiatePrefab<HudView>("HudView");
                _hudView.transform.SetParent(_uiProvider.HudLayer, false);
            }

            _hudView.gameObject.SetActive(true);
            _hudController ??= GetHudController();
            _hudController.OnShowHud(_hudView);
        }

        public void HideHud()
        {
            _hudView.gameObject.SetActive(false);
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