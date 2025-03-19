using Infrastructure.Data.Level;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI.Items;

namespace Infrastructure.Factories.Level
{
    public interface ILevelFactory
    {
        LevelModel CreateModel(LevelStaticData levelData);
        LevelPreviewModel CreatePreviewModel(LevelStaticData levelData);
    }
}