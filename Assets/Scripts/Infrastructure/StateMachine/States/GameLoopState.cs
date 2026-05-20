using Infrastructure.Constants;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Hud;
using Infrastructure.Services.Level;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class GameLoopState : State
    {
        private readonly ILevelsService _levelsService;
        private readonly IHudService _hudService;
        private readonly IWindowService _windowService;
        private readonly IAdsService _adsService;

        public GameLoopState(IHudService hudService, IWindowService windowService, IAdsService adsService)
        {
            _hudService = hudService;
            _windowService = windowService;
            _adsService = adsService;
        }

        public override async void Enter()
        {
            await _hudService.ShowHud();
            _windowService.HideActiveWindow();
            
            #if !UNITY_EDITOR
            _adsService.ShowAds(AdsId.Banner);
            #endif
        }

        public override void Exit()
        {
            _hudService.HideHud();
            _windowService.HideActiveWindow();
            _adsService.HideBanner();
        }
    }
}