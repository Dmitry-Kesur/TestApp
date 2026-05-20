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
        private readonly LevelProgressController _levelProgressController;
        private readonly IItemsStrategyFactory _itemsStrategyFactory;
        private readonly ItemsSpawnController _itemsSpawnController;
        private readonly ItemsInteractionController _itemsInteractionController;

        public event Action OnLoseAction;
        public event Action OnWinAction;
        public event Action OnStartedAction;
        public event Action OnStoppedAction;
        public event Action<ItemView> OnSpawnItemAction;

        private bool _started;

        private LevelStaticData _levelStaticData;

        public LevelModel(ItemsSpawnController itemsSpawnController, ItemsInteractionController itemsInteractionController, LevelProgressController levelProgressController, IItemsStrategyFactory itemsStrategyFactory)
        {
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

        public void SetData(LevelStaticData levelStaticData) =>
            _levelStaticData = levelStaticData;

        public void Start()
        {
            _started = true;
            ClearAllControllers();
            
            SetupProgressController();
            SetupItemsSpawnController();

            OnStartedAction?.Invoke();
            
            SetItemsToControllers();

            _itemsSpawnController.OnStartLevel();
        }

        public void Pause()
        {
            _itemsSpawnController.Pause();
        }
        
        public void Resume()
        {
            _itemsSpawnController.Resume();
        }

        public void Stop()
        {
            _started = false;
            _itemsSpawnController.Clear();
            OnStoppedAction?.Invoke();
        }

        public void Revive()
        {
            _levelProgressController.ClearFailedProgress();
            _itemsSpawnController.Resume();
        }

        private void ClearAllControllers()
        {
            _itemsSpawnController.Clear();
            _itemsInteractionController.Clear();
            _levelProgressController.Clear();
        }

        private void OnLose()
        {
            _itemsSpawnController.Pause();
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

        private void SetItemsToControllers()
        {
            var levelItemsStrategy = _itemsStrategyFactory.GetItemsStrategy(_levelStaticData.LevelItemIds);
            var items = levelItemsStrategy.GetItems();
            
            _itemsSpawnController.SetItems(items);
            _itemsInteractionController.SetItems(items);
        }
    }
}