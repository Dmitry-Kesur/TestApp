using Infrastructure.Data.Level;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI;

namespace Infrastructure.Factories
{
    public interface ILevelFactory
    {
        LevelModel CreateModel(LevelStaticData levelData);
        LevelPreviewModel CreatePreviewModel(LevelStaticData levelData);
    }
}