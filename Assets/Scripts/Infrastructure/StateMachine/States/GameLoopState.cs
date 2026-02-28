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
        private readonly IAnalyticsService _analyticsService;
        private readonly IAdsService _adsService;

        public GameLoopState(ILevelsService levelsService, IHudService hudService, IWindowService windowService, IAnalyticsService analyticsService, IAdsService adsService)
        {
            _levelsService = levelsService;
            _hudService = hudService;
            _windowService = windowService;
            _analyticsService = analyticsService;
            _adsService = adsService;
        }

        public override async void Enter()
        {
            await _hudService.ShowHud();
            _windowService.HideActiveWindow();
            
            #if !UNITY_EDITOR
            _adsService.ShowAds(AdsId.Banner);
            #endif

            _levelsService.OnEnterGameLoop();
        }

        public override void Exit()
        {
            _hudService.HideHud();
            _windowService.HideActiveWindow();
            _adsService.HideBanner();
        }
    }
}