using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.DailyBonus;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Preloader;
using Infrastructure.Factories;
using Infrastructure.Models.GameEntities.DailyBonus;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Resource;
using Infrastructure.Utils;

namespace Infrastructure.Services.DailyBonus
{
    public class DailyBonusService : ILoadableService, IBootstrapTarget
    {
        private const int MaxAvailableDaysCount = 3;
        
        public event Action OnRewardTaken;
        
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly LocalAddressableService _addressableService;
        private readonly DailyBonusFactory _dailyBonusFactory;
        private readonly ResourcesService _resourcesService;
        private readonly INotificationService _notificationService;

        private int _streak;
        private int _activeDayIndex;
        
        private List<DailyBonusDayData> _daysData;
        private List<DailyBonusDayModel> _dayModels;

        public DailyBonusService(ISaveLoadProgressService saveLoadProgressService, LocalAddressableService addressableService, DailyBonusFactory dailyBonusFactory, ResourcesService resourcesService, INotificationService notificationService)
        {
            _saveLoadProgressService = saveLoadProgressService;
            _addressableService = addressableService;
            _dailyBonusFactory = dailyBonusFactory;
            _resourcesService = resourcesService;
            _notificationService = notificationService;
        }

        public async Task Load()
        {
            _daysData = await _addressableService.LoadScriptableCollectionFromGroupAsync<DailyBonusDayData>(AddressableGroupNames
                .DailyBonusGroup);
        }

        public LoadingStage LoadingStage => LoadingStage.LoadingDailyBonus;
        
        public int InitializationOrder => 5;

        public void Initialize()
        {
            CreateDailyBonusModels();
            UpdateDailyBonusState();
        }

        public bool CanTakeReward => _dayModels.Any(model => model.CanTakeReward);

        public IReadOnlyList<DailyBonusDayModel> AvailableDayModels
        {
            get
            {
                int startIndex = _activeDayIndex;
                int maxStart = _dayModels.Count - MaxAvailableDaysCount;
                if (startIndex > maxStart)
                    startIndex = maxStart;

                return _dayModels.GetRange(startIndex, MaxAvailableDaysCount);
            }
        }

        private void CreateDailyBonusModels()
        {
            _dayModels = new List<DailyBonusDayModel>();
            _daysData.Sort((dayA, dayB) => dayA.Day.CompareTo(dayB.Day));
            
            foreach (var dayData in _daysData)
            {
                var dayModel = _dailyBonusFactory.CreateDayModel(dayData);
                var rewardResource = _resourcesService.GetResourceById(dayData.RewardResourceId);
                dayModel.IconSprite = rewardResource.Icon;
                dayModel.OnTakeRewardAction += OnTakeDayReward;
                _dayModels.Add(dayModel);
            }
        }

        private void UpdateDailyBonusState()
        {
            ResetDaysTakeRewardStatus();

            int daysCount = _dayModels.Count;
            if (daysCount == 0)
                return;

            var today = DateUtils.TodayUtcInt();
            var yesterday = DateUtils.YesterdayUtcInt();

            var lastClaimDay = _saveLoadProgressService.Read(progressData => progressData.DailyBonusLastClaimDay);
            var savedStreak  = _saveLoadProgressService.Read(progressData => progressData.DailyBonusStreak);

            bool canClaim = lastClaimDay != today;

            _streak = 1;

            if (lastClaimDay == today)
            {
                _streak = savedStreak;
            }
            else if (lastClaimDay == yesterday)
            {
                _streak = savedStreak + 1;
                
                if (_streak > daysCount)
                    _streak = 1;
            }

            _activeDayIndex = _streak - 1;

            if (canClaim)
            {
                _dayModels[_activeDayIndex].CanTakeReward = true;
            }
        }

        private void ResetDaysTakeRewardStatus()
        {
            foreach (var dayModel in _dayModels)
                dayModel.CanTakeReward = false;
        }

        private void OnTakeDayReward(DailyBonusDayModel dayModel)
        {
            if (!dayModel.CanTakeReward)
                return;

            _resourcesService.AddResource(dayModel.RewardResourceId, dayModel.RewardAmount);

            _saveLoadProgressService.Write(progress =>
            {
                progress.DailyBonusLastClaimDay = DateUtils.TodayUtcInt();
                progress.DailyBonusStreak = _streak;
            });

            UpdateDailyBonusState();

            ShowRewardTakenNotification();
            
            OnRewardTaken?.Invoke();
        }

        private void ShowRewardTakenNotification()
        {
            var notificationModel = new NotificationWithTextModel();
            notificationModel.NotificationText = UIMessages.TakenDailyBonusRewardAlias;
            _notificationService.ShowNotification(notificationModel);
        }
    }
}