using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Boosters;
using Infrastructure.Data.Preloader;
using Infrastructure.Factories.Booster;
using Infrastructure.Models.GameEntities.Boosters;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Resource;

namespace Infrastructure.Services.Booster
{
    public class BoostersService : IBoostersService, ILoadableService, IBootstrapTarget
    {
        private readonly List<BoosterModel> _boosterModels = new();

        private readonly LocalAddressableService _localAddressableService;
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly ResourcesService _resourcesService;
        private readonly BoosterFactory _boosterFactory;
        private readonly Timer _activeBoosterTimer;

        private List<BoosterData> _boostersData = new();

        private BoosterModel _activeBoosterModel;

        public BoostersService(LocalAddressableService localAddressableService,
            ISaveLoadProgressService saveLoadProgressService,
            ResourcesService resourcesService, BoosterFactory boosterFactory, Timer activeBoosterTimer)
        {
            _localAddressableService = localAddressableService;
            _saveLoadProgressService = saveLoadProgressService;
            _resourcesService = resourcesService;
            _boosterFactory = boosterFactory;
            _activeBoosterTimer = activeBoosterTimer;
        }

        public int BoostValue =>
            _activeBoosterModel?.BoostValue ?? 0;

        public BoosterModel ActiveBooster =>
            _activeBoosterModel;

        public IReadOnlyList<BoosterModel> GetAvailableBoosters() =>
            _boosterModels.FindAll(model => model.IsEnough);

        public Action OnBoosterActivatedAction { get; set; }
        
        public Action OnBoosterDeactivatedAction { get; set; }

        public async Task Load()
        {
            _boostersData =
                await _localAddressableService.LoadScriptableCollectionFromGroupAsync<BoosterData>(AddressableGroupNames
                    .BoostersGroup);
        }

        public int InitializationOrder => 5;

        public void Initialize()
        {
            CreateBoosterModels();
            UpdateBoosterState();
        }

        public LoadingStage LoadingStage =>
            LoadingStage.LoadingBoosters;

        public bool HasBoosterToActivate =>
            _boosterModels.Any(model => model.IsEnough) &&
            _activeBoosterModel == null;

        private void CreateBoosterModels()
        {
            foreach (var boosterData in _boostersData)
            {
                var boosterResource = _resourcesService.GetResourceById(boosterData.RequiredResourceId);
                var boosterModel = _boosterFactory.Create(boosterData, boosterResource);
                boosterModel.ActivateBoosterAction += ActivateBooster;
                _boosterModels.Add(boosterModel);
            }
        }

        private void UpdateBoosterState()
        {
            var activeBoosterId = _saveLoadProgressService.Read(progress => progress.ActiveBoosterId);
            if (activeBoosterId == 0)
                return;

            var endUnixSeconds = _saveLoadProgressService.Read(progress => progress.ActiveBoosterEndUnixSeconds);
            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (nowUnix >= endUnixSeconds)
            {
                DeactivateBooster();
                return;
            }

            var boosterModel = _boosterModels.Find(model => model.Id == activeBoosterId);
            _activeBoosterModel = boosterModel;
            OnBoosterActivatedAction?.Invoke();

            ScheduleBoosterEnd(endUnixSeconds);
        }

        private void ScheduleBoosterEnd(long endUnixSeconds)
        {
            _activeBoosterTimer.OnTimerEnd += OnBoosterTimeEnd;
            var remainingSeconds = endUnixSeconds - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _activeBoosterTimer.Start(remainingSeconds);
        }

        private void OnBoosterTimeEnd()
        {
            _activeBoosterTimer.OnTimerEnd -= OnBoosterTimeEnd;
            DeactivateBooster();
        }

        private void ActivateBooster(BoosterModel boosterModel)
        {
            _resourcesService.SpendResource(boosterModel.RequiredResourceId, 1);

            var endUnixSeconds = CalculateEndUnixSeconds(boosterModel.DurationSeconds);

            _saveLoadProgressService.Write(progress =>
            {
                progress.ActiveBoosterId = boosterModel.Id;
                progress.ActiveBoosterEndUnixSeconds = endUnixSeconds;
            });

            _activeBoosterModel = boosterModel;
            ScheduleBoosterEnd(endUnixSeconds);

            OnBoosterActivatedAction?.Invoke();
        }

        private void DeactivateBooster()
        {
            _activeBoosterModel = null;
            _saveLoadProgressService.Write(progress =>
            {
                progress.ActiveBoosterId = 0;
                progress.ActiveBoosterEndUnixSeconds = 0;
            });
            
            OnBoosterDeactivatedAction?.Invoke();
        }

        private long CalculateEndUnixSeconds(int durationSeconds) =>
            DateTimeOffset.UtcNow.ToUnixTimeSeconds() + durationSeconds;
    }
}