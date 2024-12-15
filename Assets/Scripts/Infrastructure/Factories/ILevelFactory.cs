using Infrastructure.Data.Level;
using Infrastructure.Models.GameEntities.Level;

namespace Infrastructure.Factories
{
    public interface ILevelFactory
    {
        LevelModel CreateModel(LevelStaticData levelData);
    }
}