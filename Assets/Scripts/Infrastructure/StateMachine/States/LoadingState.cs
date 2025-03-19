using System.Threading.Tasks;
using Infrastructure.Enums;
using Infrastructure.Providers;
using Infrastructure.Providers.InAppPurchase;
using Infrastructure.Services;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class LoadingState : State
    {
        private readonly IWindowService _windowService;
        private readonly IPreloaderService _preloaderService;
        private readonly InAppPurchaseProvider _inAppPurchaseProvider;

        public LoadingState(IWindowService windowService, IPreloaderService preloaderService, InAppPurchaseProvider inAppPurchaseProvider)
        {
            _windowService = windowService;
            _preloaderService = preloaderService;
            _inAppPurchaseProvider = inAppPurchaseProvider;
        }

        public override async void Enter()
        {
            _windowService.ShowWindow(WindowId.PreloaderWindow);
            await LoadServices();
        }

        public override void Exit()
        {
            _windowService.HideWindow(WindowId.PreloaderWindow);
        }

        private async Task LoadServices()
        {
            await _preloaderService.Load();
            AfterLoadServices();
        }

        private void AfterLoadServices()
        {
            _inAppPurchaseProvider.Initialize();
        }
    }
}