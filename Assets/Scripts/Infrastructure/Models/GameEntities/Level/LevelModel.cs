using System;
using System.Collections.Generic;
using Infrastructure.Controllers.Levels;
using Infrastructure.Data.Level;
using Infrastructure.Data.Rewards;
using Infrastructure.Factories.Level;
using Infrastructure.Views.GameEntities;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.Level
{
    public class LevelModel : ILevelModel
    {
        private readonly LevelViewsFactory _levelViewsFactory;
        private readonly LevelProgressController _levelProgressController;
        private readonly IItemsStrategyFactory _itemsStrategyFactory;
        private readonly ItemsSpawnController _itemsSpawnController;
        private readonly ItemsInteractionController _itemsInteractionController;

        public Action OnLoseAction;
        public Action OnWinAction;
        public Action<ItemView> OnSpawnItemAction;

        private bool _started;

        private LevelStaticData _levelStaticData;

        public LevelModel(LevelViewsFactory levelViewsFactory,
            ItemsSpawnController itemsSpawnController, ItemsInteractionController itemsInteractionController, LevelProgressController levelProgressController, IItemsStrategyFactory itemsStrategyFactory)
        {
            _levelViewsFactory = levelViewsFactory;
            _itemsSpawnController = itemsSpawnController;
            _itemsInteractionController = itemsInteractionController;
            _levelProgressController = levelProgressController;
            _itemsStrategyFactory = itemsStrategyFactory;
        }

        public int Level =>
            _levelStaticData.Level;

        public int TotalLevelScore =>
            _levelProgressController.TotalLevelScore;

        public int TotalFailItems =>
            _levelProgressController.TotalFailItems;

        public int CatchItemsToDecreaseSpawnDelay =>
            _levelStaticData.CatchItemsToDecreaseSpawnDelay;

        public int ScorePointsToWin =>
            _levelStaticData.ScorePointsToWin;

        public int MaximumFailItems =>
            _levelStaticData.MaximumFailItems;

        public float DefaultItemsSpawnDelay =>
            _levelStaticData.DefaultItemsSpawnDelay;

        public float DefaultDropItemsDuration =>
            _levelStaticData.DefaultDropItemsDuration;

        public float MinimalDropItemsDuration =>
            _levelStaticData.MinimalDropItemsDuration;

        public float MinimalItemsSpawnDelay =>
            _levelStaticData.MinimalItemsSpawnDelay;

        public float SpawnDelayDecreaseValue =>
            _levelStaticData.SpawnDelayDecreaseValue;

        public float DropItemsDecreaseDurationValue =>
            _levelStaticData.DropItemsDecreaseDurationValue;

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

            AfterSetData();
        }

        private void AfterSetData()
        {
            SetupProgressController();
            SetupItemsSpawnController();
        }

        public void Start()
        {
            _started = true;
            ClearAllControllers();

            _levelViewsFactory.CreateLevelView(this);
            
            ApplyItemsToControllers();

            _itemsSpawnController.OnStartLevel();
        }

        public void OnPause() =>
            _itemsSpawnController.OnPause();

        public void OnResume()
        {
            _levelProgressController.Refresh();
            _itemsSpawnController.OnResume();
        }

        public void Stop()
        {
            _started = false;
            _itemsSpawnController.Clear();
            _levelViewsFactory.DestroyLevelView();
        }

        private void ClearAllControllers()
        {
            _itemsSpawnController.Clear();
            _itemsInteractionController.Clear();
            _levelProgressController.Clear();
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

        private void SetupProgressController()
        {
            _levelProgressController.SetModel(this);
            _levelProgressController.OnReachScoreToWin = OnWin;
            _levelProgressController.OnReachedMaximumFailItems = OnLose;
        }

        private void SetupItemsSpawnController()
        {
            _itemsSpawnController.SetModel(this);
            _itemsSpawnController.OnSpawnItemAction = OnSpawnItem;
        }

        private void OnSpawnItem(ItemView itemView) =>
            OnSpawnItemAction?.Invoke(itemView);

        private void ApplyItemsToControllers()
        {
            var levelItemsStrategy = _itemsStrategyFactory.GetItemsStrategy(_levelStaticData.LevelItemIds);
            var items = levelItemsStrategy.GetItems();
            
            _itemsSpawnController.SetItems(items);
            _itemsInteractionController.SetItems(items);
        }
    }
}