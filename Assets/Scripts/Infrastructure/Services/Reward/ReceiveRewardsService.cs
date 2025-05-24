using System.Collections.Generic;
using Infrastructure.Constants;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Rewards;
using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Rewards;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Currency;
using Infrastructure.Services.Notification;

namespace Infrastructure.Services.Reward
{
    public class ReceiveRewardsService : IReceiveRewardsService
    {
        private readonly IRewardsService _rewardsService;
        private readonly INotificationService _notificationService;
        private readonly IAnalyticsService _analyticsService;
        private readonly ICurrencyService _currencyService;

        public ReceiveRewardsService(ICurrencyService currencyService, IRewardsService rewardsService,
            INotificationService notificationService, IAnalyticsService analyticsService)
        {
            _currencyService = currencyService;
            _rewardsService = rewardsService;
            _notificationService = notificationService;
            _analyticsService = analyticsService;
        }

        public void ReceiveRewards(List<RewardReceiveData> receiveRewards)
        {
            var rewardModels = _rewardsService.GetRewardsModelsByData(receiveRewards);

            if (rewardModels == null)
                return;
            
            foreach (var rewardModel in rewardModels)
            {
                if (rewardModel.Type == RewardType.Currency)
                {
                    _currencyService.IncreaseCurrency(rewardModel.Amount);
                }
                
                _analyticsService.LogReceiveReward(rewardModel.Type);
            }

            ShowRewardsNotification(rewardModels);
        }

        private void ShowRewardsNotification(List<RewardModel> rewards)
        {
            var notificationModel = new NotificationWithRewardsModel(rewards)
            {
                NotificationText = UIMessages.ReceiveRewardAlias
            };
            _notificationService.ShowNotification(notificationModel);
        }
    }
}