using System.Collections.Generic;
using Infrastructure.Constants;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Rewards;
using Infrastructure.Models.GameEntities.Rewards;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Resource;

namespace Infrastructure.Services.Reward
{
    public class ReceiveRewardsService : IReceiveRewardsService
    {
        private readonly ResourcesService _resourcesService;
        private readonly IRewardsService _rewardsService;
        private readonly INotificationService _notificationService;
        private readonly IAnalyticsService _analyticsService;

        public ReceiveRewardsService(ResourcesService resourcesService, IRewardsService rewardsService,
            INotificationService notificationService, IAnalyticsService analyticsService)
        {
            _resourcesService = resourcesService;
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
                _resourcesService.AddResource(rewardModel.Id, rewardModel.Amount);
                
                _analyticsService.LogReceiveReward(rewardModel.Id);
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