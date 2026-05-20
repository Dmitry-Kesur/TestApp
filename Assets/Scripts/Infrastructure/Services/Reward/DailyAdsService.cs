using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Preloader;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Resource;
using Infrastructure.Utils;

namespace Infrastructure.Services.Reward
{
    public class DailyAdsService : IBootstrapTarget, ILoadableService
    {
        private readonly AdsService _adsService;
        private readonly ResourcesService _resourcesService;
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly INotificationService _notificationService;
        private readonly LocalAddressableService _localAddressableService;

        private bool _showDailyAd;
        
        private DailyAdsRewardData _dailyAdsRewardData;

        public DailyAdsService(AdsService adsService, ResourcesService resourcesService, ISaveLoadProgressService saveLoadProgressService, INotificationService notificationService, LocalAddressableService localAddressableService)
        {
            _adsService = adsService;
            _resourcesService = resourcesService;
            _saveLoadProgressService = saveLoadProgressService;
            _notificationService = notificationService;
            _localAddressableService = localAddressableService;
        }
        
        public async Task Load()
        {
            _dailyAdsRewardData = await _localAddressableService.LoadScriptableAsync<DailyAdsRewardData>(AddressableKeys.DailyAdsReward);
        }

        public LoadingStage LoadingStage => LoadingStage.DailyAdsRewards;
        
        public void Initialize()
        {
            UpdateShowDailyAdStatus();
        }
        
        public int InitializationOrder => 3;

        public void ShowRewardedAds()
        {
            if (!_showDailyAd)
            {
                ShowErrorNotification();
                return;
            }
            
            _adsService.ShowAds(AdsId.Rewarded, completeCallback: OnShowCompleteAds);
        }

        private void ShowErrorNotification()
        {
            var notification = new NotificationWithTextModel
            {
                NotificationText = UIMessages.ShowDailyRewardAdErrorAlias
            };
            _notificationService.ShowNotification(notification);
        }

        private void OnShowCompleteAds()
        {
            _resourcesService.AddResource(_dailyAdsRewardData.ResourceId, _dailyAdsRewardData.Amount);
            MarkShowAdToday();
            
            ShowCompleteRewardNotification();
        }

        private void ShowCompleteRewardNotification()
        {
            var rewardResource = _resourcesService.GetResourceById(_dailyAdsRewardData.ResourceId);
            var notification = new NotificationWithIconModel
            {
                NotificationIcon = rewardResource.Icon,
                NotificationText = UIMessages.CompleteDailyAdRewardAlias
            };
            _notificationService.ShowNotification(notification);
        }

        private void MarkShowAdToday()
        {
            _saveLoadProgressService.Write(progress => progress.LastShowDailyAdRewardDay = DateUtils.TodayUtcInt());
            _showDailyAd = false;
        }

        private void UpdateShowDailyAdStatus()
        {
            var today = DateUtils.TodayUtcInt();
            var lastShowDailyAdRewardDay = _saveLoadProgressService.Read(progress => progress.LastShowDailyAdRewardDay);
            
            _showDailyAd = lastShowDailyAdRewardDay != today;
        }
    }
}