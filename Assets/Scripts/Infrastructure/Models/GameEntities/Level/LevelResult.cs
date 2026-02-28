namespace Infrastructure.Models.GameEntities.Level
{
    public struct LevelResult
    {
        public readonly int Score;

        public LevelResult(int score)
        {
            Score = score;
        }
    }
}