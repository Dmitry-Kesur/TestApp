using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.LuckySpin;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Preloader;
using Infrastructure.Factories;
using Infrastructure.Models.UI;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Resource;
using Infrastructure.Utils;
using Random = UnityEngine.Random;

namespace Infrastructure.Services.LuckySpin
{
    public class LuckySpinService : ILoadableService, IBootstrapTarget
    {
        public event Action<int> OnStartSpinAction;
        public event Action RefreshAction;

        private readonly LocalAddressableService _localAddressableService;
        private readonly LuckySpinFactory _luckySpinFactory;
        private readonly ResourcesService _resourcesService;
        private readonly INotificationService _notificationService;
        private readonly AdsService _adsService;
        private readonly ISaveLoadProgressService _saveLoadProgressService;

        private bool _canFreeSpin;

        private List<WheelSegmentModel> _segmentModels;
        private List<WheelSegmentData> _segmentsData;

        public LuckySpinService(LocalAddressableService localAddressableService, LuckySpinFactory luckySpinFactory,
            ResourcesService resourcesService, INotificationService notificationService, AdsService adsService,
            ISaveLoadProgressService saveLoadProgressService)
        {
            _localAddressableService = localAddressableService;
            _luckySpinFactory = luckySpinFactory;
            _resourcesService = resourcesService;
            _notificationService = notificationService;
            _adsService = adsService;
            _saveLoadProgressService = saveLoadProgressService;
        }

        public async Task Load()
        {
            _segmentsData =
                await _localAddressableService.LoadScriptableCollectionFromGroupAsync<WheelSegmentData>(
                    AddressableGroupNames.LuckySpinWheelSegmentsGroup);
            _segmentsData.Sort((dataA, dataB) => dataA.Index - dataB.Index);
        }

        public LoadingStage LoadingStage => LoadingStage.LoadingLuckySpin;

        public int InitializationOrder => 6;

        public bool CanFreeSpin => _canFreeSpin;

        public List<WheelSegmentModel> SegmentModels => _segmentModels;

        public void Initialize()
        {
            CreateWheelSegmentModels();
            UpdateLuckySpinState();
        }

        public void StartFreeSpin()
        {
            if (!_canFreeSpin)
                return;

            StartSpin();
        }

        public void StartSpinForRewardedAd()
        {
            if (_canFreeSpin)
                return;

            void OnAdsShowCompleted(string adsId)
            {
                if (adsId != AdsId.Rewarded)
                    return;

                _adsService.OnAdsShowCompletedAction -= OnAdsShowCompleted;
                StartSpin();
            }

            _adsService.OnAdsShowCompletedAction += OnAdsShowCompleted;
            _adsService.ShowAds(AdsId.Rewarded);
        }

        private void StartSpin()
        {
            var segmentIndex = GetSegmentIndexByWeight();
            OnStartSpinAction?.Invoke(segmentIndex);
        }

        public void OnCompleteSpin(int segmentIndex)
        {
            var segmentModel = _segmentModels.Find(model => model.Index == segmentIndex);
            if (segmentModel == null)
                return;

            GetReward(segmentModel);

            MarkLuckySpinUsed();

            RefreshAction?.Invoke();
        }

        private void GetReward(WheelSegmentModel segmentModel)
        {
            _resourcesService.AddResource(segmentModel.RewardId, segmentModel.RewardAmount);
            var rewardResource = _resourcesService.GetResourceById(segmentModel.RewardId);

            var notificationModel = new NotificationWithIconModel
                { NotificationIcon = rewardResource.Icon, NotificationText = segmentModel.RewardAmount.ToString() };
            _notificationService.ShowNotification(notificationModel);
        }

        private int GetSegmentIndexByWeight()
        {
            float totalWeight = 0f;
            foreach (var segmentModel in _segmentModels)
                totalWeight += segmentModel.Weight;

            float randomWeight = Random.Range(0f, totalWeight);

            float accumulated = 0f;
            for (int i = 0; i < _segmentModels.Count; i++)
            {
                accumulated += _segmentModels[i].Weight;
                if (randomWeight <= accumulated)
                    return i;
            }

            return _segmentModels.Count - 1;
        }

        private void CreateWheelSegmentModels()
        {
            _segmentModels = new List<WheelSegmentModel>();

            foreach (var segmentData in _segmentsData)
            {
                var segmentModel = _luckySpinFactory.CreateWheelSegmentModel(segmentData);
                var rewardResource = _resourcesService.GetResourceById(segmentData.RewardId);
                segmentModel.IconSprite = rewardResource.Icon;
                _segmentModels.Add(segmentModel);
            }
        }

        private void UpdateLuckySpinState()
        {
            var today = DateUtils.TodayUtcInt();
            var lastLuckySpinUseDay = _saveLoadProgressService.Read(progress => progress.LastLuckySpinUseDay);
            _canFreeSpin = lastLuckySpinUseDay != today;
        }

        private void MarkLuckySpinUsed()
        {
            _saveLoadProgressService.Write(progress => progress.LastLuckySpinUseDay = DateUtils.TodayUtcInt());
            _canFreeSpin = false;
        }
    }
}