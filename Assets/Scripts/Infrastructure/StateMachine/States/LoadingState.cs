using Infrastructure.Enums;
using Infrastructure.Providers.InAppPurchase;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.InGamePurchase;
using Infrastructure.Services.Items;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class LoadingState : State
    {
        private readonly IWindowService _windowService;
        private readonly IPreloaderService _preloaderService;
        private readonly BootstrapService _bootstrapService;
        private readonly InAppPurchaseProvider _inAppPurchaseProvider;
        private readonly ShopService _shopService;
        private readonly IItemsService _itemsService;

        public LoadingState(IWindowService windowService, IPreloaderService preloaderService,
            BootstrapService bootstrapService)
        {
            _windowService = windowService;
            _preloaderService = preloaderService;
            _bootstrapService = bootstrapService;
        }

        public override async void Enter()
        {
            _windowService.ShowWindow(WindowId.PreloaderWindow);
            await _preloaderService.Load();
            _bootstrapService.Initialize();
        }

        public override void Exit()
        {
            _windowService.HideWindow(WindowId.PreloaderWindow);
        }
    }
}