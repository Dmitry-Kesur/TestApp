using Infrastructure.Constants;
using Infrastructure.Data.Notifications;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Resource;
using Infrastructure.Utils;

namespace Infrastructure.Services.Reward
{
    public class DailyAdRewardService : IBootstrapTarget
    {
        private const int RewardId = 1;
        private const int RewardAmountPerDay = 5;
        
        private readonly AdsService _adsService;
        private readonly ResourcesService _resourcesService;
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly INotificationService _notificationService;

        private bool _showDailyAd;

        public DailyAdRewardService(AdsService adsService, ResourcesService resourcesService, ISaveLoadProgressService saveLoadProgressService, INotificationService notificationService)
        {
            _adsService = adsService;
            _resourcesService = resourcesService;
            _saveLoadProgressService = saveLoadProgressService;
            _notificationService = notificationService;
        }

        public int InitializationOrder => 3;
        
        public void Initialize()
        {
            UpdateShowDailyAdStatus();
        }

        public void ShowRewardedAds()
        {
            if (!_showDailyAd)
            {
                var notification = new NotificationWithTextModel
                {
                    NotificationText = UIMessages.ShowDailyRewardAdErrorAlias
                };
                _notificationService.ShowNotification(notification);
                
                return;
            }
            
            _adsService.OnAdsShowCompletedAction += OnShowCompleteAds;
            _adsService.ShowAds(AdsId.Rewarded);
        }

        private void OnShowCompleteAds(string adsId)
        {
            if (adsId != AdsId.Rewarded)
                return;
            
            _adsService.OnAdsShowCompletedAction -= OnShowCompleteAds;
            _resourcesService.AddResource(RewardId, RewardAmountPerDay);
            MarkShowAdToday();
            
            ShowCompleteRewardNotification();
        }

        private void ShowCompleteRewardNotification()
        {
            var rewardResource = _resourcesService.GetResourceById(RewardId);
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