using System;
using System.Collections.Generic;
using Infrastructure.Controllers.Levels;
using Infrastructure.Data.Level;
using Infrastructure.Data.Rewards;
using Infrastructure.Enums;
using Infrastructure.Factories;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services;
using Infrastructure.Views.GameEntities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Models.GameEntities.Level
{
    public class LevelModel : ILevelModel
    {
        private readonly ILevelViewsFactory _levelViewsFactory;
        private readonly ISoundService _soundService;
        private readonly ProgressController _progressController;
        private readonly ICrashlyticsService _crashlyticsService;
        private readonly ItemsSpawnController _itemsSpawnController;

        public Action OnLoseAction;
        public Action OnWinAction;
        public Action<ItemView> OnSpawnItemAction;

        private bool _started;

        private List<ItemModel> _itemModels;

        private LevelStaticData _levelStaticData;

        private LevelView _levelView;


        public LevelModel(ILevelViewsFactory levelViewsFactory, ISoundService soundService,
            ItemsSpawnController itemsSpawnController, ProgressController progressController,
            ICrashlyticsService crashlyticsService)
        {
            _levelViewsFactory = levelViewsFactory;
            _soundService = soundService;
            _itemsSpawnController = itemsSpawnController;
            _progressController = progressController;
            _crashlyticsService = crashlyticsService;
        }

        public int Level =>
            _levelStaticData.Level;

        public int ScorePointsToWin =>
            _levelStaticData.ScorePointsToWin;

        public int CatchItemsToDecreaseSpawnDelay =>
            _levelStaticData.CatchItemsToDecreaseSpawnDelay;

        public int TotalFailItems =>
            _progressController.TotalFailItems;

        public int TotalLevelScore =>
            _progressController.TotalLevelScore;

        public float DefaultItemsSpawnDelay =>
            _levelStaticData.DefaultItemsSpawnDelay;

        public float MinimalItemsSpawnDelay =>
            _levelStaticData.MinimalItemsSpawnDelay;

        public float SpawnDelayDecreaseValue =>
            _levelStaticData.SpawnDelayDecreaseValue;

        public float DefaultDropItemsDuration =>
            _levelStaticData.DefaultDropItemsDuration;

        public float MinimalDropItemsDuration =>
            _levelStaticData.MinimalDropItemsDuration;

        public float DropItemsDecreaseDurationValue =>
            _levelStaticData.DropItemsDecreaseDurationValue;

        public int FailItemsMaximum =>
            _levelStaticData.MaximumFailItems;

        public Sprite LevelBackground =>
            _levelStaticData.LevelBackground;

        public float DropItemsDuration =>
            _itemsSpawnController.GetDropItemsDuration();

        public bool Started =>
            _started;

        public List<RewardReceiveData> GetRewards() =>
            _levelStaticData.LevelRewards;

        public void SetData(LevelStaticData levelStaticData)
        {
            _levelStaticData = levelStaticData;

            AssignControllers();
        }

        public void OnStart()
        {
            _started = true;
            _progressController.Clear();

            RenderLevel();
            SetupLevelParameters();

            _itemsSpawnController.OnStartLevel();

            OnUpdateProgress();
        }

        public void OnPause() =>
            _itemsSpawnController.OnPauseLevel();

        public void OnResume()
        {
            _itemsSpawnController.OnResumeLevel();
            OnUpdateProgress();
        }

        private void OnFailItem()
        {
            _progressController.UpdateProgressByFailItem();

            _soundService.PlaySound(SoundId.Fail);
        }

        public void Stop()
        {
            _started = false;
            _itemsSpawnController.Clear();
            OnDestroyLevel();
        }

        private void SetupLevelParameters() =>
            _itemsSpawnController.SetItemsByIds(_levelStaticData.LevelItemIds);

        private void OnCatchItem(ItemModel itemModel)
        {
            _progressController.UpdateProgressByCatchItem(itemModel.ScorePoints);

            _soundService.PlaySound(SoundId.Catch);
        }

        private void OnLose()
        {
            Stop();
            OnLoseAction?.Invoke();
        }

        private void OnWin()
        {
            OnWinAction?.Invoke();
            Stop();
        }

        private void RenderLevel()
        {
            if (_levelView != null)
                return;

            _levelView = _levelViewsFactory.CreateLevelView();
            _levelView.SetModel(this);
        }

        private void OnDestroyLevel()
        {
            if (_levelView == null)
            {
                var exceptionText = "Level View is null";
                _crashlyticsService.LogError(exceptionText);
                throw new NullReferenceException(exceptionText);
            }

            Object.Destroy(_levelView.gameObject);
            _levelView = null;
        }

        private void OnUpdateProgress() =>
            _itemsSpawnController.UpdateItemsByTotalCatchAmount(_progressController.TotalCatchItems);

        private void AssignControllers()
        {
            _progressController.SetModel(this);
            _progressController.OnUpdateProgress = OnUpdateProgress;
            _progressController.OnReachScoreToWin = OnWin;
            _progressController.OnReachedMaximumFailItems = OnLose;

            _itemsSpawnController.SetModel(this);
            _itemsSpawnController.OnCatchItemAction = OnCatchItem;
            _itemsSpawnController.OnFailItemAction = OnFailItem;
            _itemsSpawnController.OnSpawnItemAction = OnSpawnItem;
        }

        private void OnSpawnItem(ItemView itemView) =>
            OnSpawnItemAction?.Invoke(itemView);
    }
}