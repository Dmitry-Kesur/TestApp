using System.Collections.Generic;
using Infrastructure.Data.Rewards;
using UnityEngine;

namespace Infrastructure.Data.Level
{
    public class LevelStaticData : ScriptableObject
    {
        public int Level;
        public int MaximumFailItems;
        public int ScorePointsToWin;
        public int CatchItemsToDecreaseSpawnDelay;
        public float DefaultItemsSpawnDelay;
        public float MinimalItemsSpawnDelay;
        public float SpawnDelayDecreaseValue;
        public float DefaultDropItemsDuration;
        public float MinimalDropItemsDuration;
        public float DropItemsDecreaseDurationValue;
        public Sprite LevelBackground;
        public List<int> LevelItemIds;
        public List<RewardReceiveData> LevelRewards;
    }
}