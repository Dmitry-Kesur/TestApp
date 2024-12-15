namespace Infrastructure.Models.GameEntities.Level
{
    public interface ILevelModel
    {
        public float DefaultItemsSpawnDelay { get; }
        public float DefaultDropItemsDuration { get; }
        public float MinimalDropItemsDuration { get; }
        public float MinimalItemsSpawnDelay { get; }
        public float SpawnDelayDecreaseValue { get; }
        public float DropItemsDecreaseDurationValue { get; }
        public int CatchItemsToDecreaseSpawnDelay { get; }
        public int ScorePointsToWin { get; }
        public int FailItemsMaximum { get; }
    }
}