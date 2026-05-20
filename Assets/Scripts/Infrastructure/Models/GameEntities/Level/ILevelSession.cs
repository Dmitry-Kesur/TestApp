namespace Infrastructure.Models.GameEntities.Level
{
    public interface ILevelModel
    {
        public int CatchItemsToDecreaseSpawnDelay { get; }
        public float DefaultItemsSpawnDelay { get; }
        public float DefaultDropItemsDuration { get; }
        public float MinimalDropItemsDuration { get; }
        public float MinimalItemsSpawnDelay { get; }
        public float SpawnDelayDecreaseValue { get; }
        public float DropItemsDecreaseDurationValue { get; }
    }
}