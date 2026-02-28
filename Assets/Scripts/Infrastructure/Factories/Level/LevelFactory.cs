using System.Linq;
using Infrastructure.Data.Level;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI.Items;
using Infrastructure.Services.Progress;
using Zenject;

namespace Infrastructure.Factories.Level
{
    public class LevelFactory
    {
        private readonly DiContainer _diContainer;
        private readonly SaveLoadProgressService _saveLoadProgressService;

        public LevelFactory(DiContainer diContainer, SaveLoadProgressService saveLoadProgressService)
        {
            _diContainer = diContainer;
            _saveLoadProgressService = saveLoadProgressService;
        }

        public LevelSession CreateLevelSession()
        {
            var levelModel = _diContainer.Instantiate<LevelSession>();
            return levelModel;
        }

        public LevelPreviewModel CreatePreviewModel(LevelStaticData levelData)
        {
            var (activeLevel, winLevelIds) =
                _saveLoadProgressService.Read(progress => (progress.ActiveLevel, progress.WinLevelIds));
            
            var previewModel = new LevelPreviewModel(levelData);

            if (levelData.Level == activeLevel)
                previewModel.IsActive = true;

            else if (winLevelIds.Contains(levelData.Level))
                previewModel.IsComplete = true;

            return previewModel;
        }
    }
}