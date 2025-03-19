using Infrastructure.Data.Level;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI.Items;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;
using Zenject;

namespace Infrastructure.Factories.Level
{
    public class LevelsFactory : ILevelFactory
    {
        private readonly DiContainer _diContainer;
        private readonly LevelProgressUpdater _levelProgressUpdater;

        public LevelsFactory(DiContainer diContainer, LevelProgressUpdater levelProgressUpdater)
        {
            _diContainer = diContainer;
            _levelProgressUpdater = levelProgressUpdater;
        }

        public LevelModel CreateModel(LevelStaticData levelData)
        {
            var levelModel = _diContainer.Instantiate<LevelModel>();
            levelModel.SetData(levelData);
            return levelModel;
        }

        public LevelPreviewModel CreatePreviewModel(LevelStaticData levelData)
        {
            var activeLevel = _levelProgressUpdater.GetActiveLevel();
            var completeLevels = _levelProgressUpdater.GetCompleteLevelIds();
            
            var previewModel = new LevelPreviewModel(levelData);

            if (levelData.Level == activeLevel)
                previewModel.IsActive = true;

            else if (completeLevels.Contains(levelData.Level))
                previewModel.IsComplete = true;

            return previewModel;
        }
    }
}