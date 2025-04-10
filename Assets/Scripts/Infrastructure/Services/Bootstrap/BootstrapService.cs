using Infrastructure.Providers.InAppPurchase;
using Infrastructure.Services.Booster;
using Infrastructure.Services.InGamePurchase;
using Infrastructure.Services.Items;
using Infrastructure.Services.Level;

namespace Infrastructure.Services.Bootstrap
{
    public class BootstrapService
    {
        private readonly InAppPurchaseProvider _inAppPurchaseProvider;
        private readonly IItemsService _itemsService;
        private readonly ShopService _shopService;
        private readonly ILevelsService _levelsService;
        private readonly IBoostersService _boostersService;

        public BootstrapService(InAppPurchaseProvider inAppPurchaseProvider, IItemsService itemsService, ShopService shopService, ILevelsService levelsService, IBoostersService boostersService)
        {
            _inAppPurchaseProvider = inAppPurchaseProvider;
            _itemsService = itemsService;
            _shopService = shopService;
            _levelsService = levelsService;
            _boostersService = boostersService;
        }

        public void Initialize()
        {
            _inAppPurchaseProvider.Initialize();
            _itemsService.Initialize();
            _shopService.Initialize();
            _levelsService.Initialize();
            _boostersService.Initialize();
        }
    }
}