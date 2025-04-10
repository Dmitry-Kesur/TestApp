using System.Collections.Generic;
using Infrastructure.Constants;
using Infrastructure.Controllers.Rewards;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Rewards;
using Infrastructure.Enums;
using Infrastructure.Factories.Reward;
using Infrastructure.Models.GameEntities.Rewards;
using Infrastructure.Services.Notification;

namespace Infrastructure.Services.Reward
{
    public class ReceiveRewardsService : IReceiveRewardsService
    {
        private readonly IReceiveRewardsControllersFactory _receiveRewardsControllersFactory;
        private readonly IRewardsService _rewardsService;
        private readonly INotificationService _notificationService;

        private List<BaseReceiveRewardsController> _rewardsControllers;

        public ReceiveRewardsService(IReceiveRewardsControllersFactory receiveRewardsControllersFactory, IRewardsService rewardsService, INotificationService notificationService)
        {
            _receiveRewardsControllersFactory = receiveRewardsControllersFactory;
            _rewardsService = rewardsService;
            _notificationService = notificationService;

            AssignControllers();
        }

        private void AssignControllers()
        {
            _rewardsControllers = _receiveRewardsControllersFactory.GetRewardsControllers();
        }

        public void ReceiveRewards(List<RewardReceiveData> receiveRewards)
        {
            var rewardModels = _rewardsService.GetRewardsModelsByData(receiveRewards);

            foreach (var rewardModel in rewardModels)
            {
                var rewardsController = GetRewardsControllerByType(rewardModel.Type);
                rewardsController.ReceiveReward(rewardModel);
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

        private BaseReceiveRewardsController GetRewardsControllerByType(RewardType rewardType) =>
            _rewardsControllers.Find(controller => controller.RewardType == rewardType);
    }
}