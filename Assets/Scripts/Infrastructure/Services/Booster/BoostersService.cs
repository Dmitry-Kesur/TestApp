using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Boosters;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Preloader;
using Infrastructure.Models.GameEntities.Boosters;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.InAppPurchase;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;

namespace Infrastructure.Services.Booster
{
    public class BoostersService : IBoostersService, ILoadableService
    {
        private readonly List<BoosterModel> _boosterModels = new();
        
        private readonly LocalAddressableService _localAddressableService;
        private readonly IInAppPurchaseService _inAppPurchaseService;
        private readonly ResourceProgressUpdater _resourceProgressUpdater;
        private readonly INotificationService _notificationService;

        private List<BoosterData> _boostersData = new();
        
        private BoosterModel _activeBoosterModel;

        public BoostersService(LocalAddressableService localAddressableService, IInAppPurchaseService inAppPurchaseService, ResourceProgressUpdater resourceProgressUpdater, INotificationService notificationService)
        {
            _localAddressableService = localAddressableService;
            _inAppPurchaseService = inAppPurchaseService;
            _resourceProgressUpdater = resourceProgressUpdater;
            _notificationService = notificationService;

            _inAppPurchaseService.OnCompletePurchase = OnCompletePurchaseBooster;
        }

        public List<BoosterModel> Boosters =>
            _boosterModels;

        public int BoostValue =>
            _activeBoosterModel?.BoostValue ?? 0;

        public BoosterModel ActiveBooster =>
            _activeBoosterModel;

        public async Task Load()
        {
            _boostersData = await _localAddressableService.LoadScriptableCollectionFromGroupAsync<BoosterData>(AddressableGroupNames.BoostersGroup);
        }

        public void Initialize()
        {
            CreateBoosterModels();
            SetActiveBooster();
        }
        
        public LoadingStage LoadingStage =>
            LoadingStage.LoadingBoosters;

        private void OnCompletePurchaseBooster(string boosterProductId)
        {
            var booster = GetBoosterByProductId(boosterProductId);
            if (booster == null)
                return;
            
            _resourceProgressUpdater.SetActiveBoosterId(booster.Id);
            SetActiveBooster();
            ShowActiveBoosterNotification($"Purchased booster: x{_activeBoosterModel.BoostValue}");
        }

        private void CreateBoosterModels()
        {
            foreach (var boosterData in _boostersData)
            {
                var boosterModel = new BoosterModel(boosterData)
                {
                    OnBuyBoosterAction = OnBuyBooster
                };
                _boosterModels.Add(boosterModel);
            }
        }

        private BoosterModel GetBoosterByProductId(string productId) =>
            _boosterModels.Find(model => model.ProductId == productId);

        private BoosterModel GetBoosterById(int boosterId) =>
            _boosterModels.Find(model => model.Id == boosterId);

        private void OnBuyBooster(string boosterProductId)
        {
            if (ActiveBooster != null)
            {
                ShowActiveBoosterNotification("You have active booster!");
                return;
            }
            
            _inAppPurchaseService.PurchaseProduct(boosterProductId);
        }

        private void SetActiveBooster()
        {
            var activeBoosterId = _resourceProgressUpdater.GetActiveBoosterId();
            if (activeBoosterId == 0)
                return;
            
            _activeBoosterModel = GetBoosterById(activeBoosterId);
        }

        private void ShowActiveBoosterNotification(string notificationText)
        {
            var notificationModel = new NotificationWithIconModel
            {
                NotificationText = notificationText,
                NotificationIcon = _activeBoosterModel.IconSprite
            };
                
            _notificationService.ShowNotification(notificationModel);
        }
    }
}