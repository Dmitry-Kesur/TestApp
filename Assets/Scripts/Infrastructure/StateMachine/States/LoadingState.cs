using Infrastructure.Enums;
using Infrastructure.Providers;
using Infrastructure.Services;

namespace Infrastructure.StateMachine.States
{
    public class LoadingState : State
    {
        private readonly IWindowService _windowService;
        private readonly IPreloaderService _preloaderService;
        private readonly IInAppPurchaseProvider _inAppPurchaseProvider;

        public LoadingState(IWindowService windowService, IPreloaderService preloaderService, IInAppPurchaseProvider inAppPurchaseProvider)
        {
            _windowService = windowService;
            _preloaderService = preloaderService;
            _inAppPurchaseProvider = inAppPurchaseProvider;
        }

        public override async void Enter()
        {
            _windowService.ShowWindow(WindowId.PreloaderWindow);
            await _preloaderService.Load();
            _inAppPurchaseProvider.Initialize();
        }

        public override void Exit()
        {
            _windowService.HideWindow(WindowId.PreloaderWindow);
        }
    }
}