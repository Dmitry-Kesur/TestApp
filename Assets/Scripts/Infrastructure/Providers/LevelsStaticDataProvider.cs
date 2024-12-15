using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Level;
using Infrastructure.Data.Preloader;
using Infrastructure.Services;

namespace Infrastructure.Providers
{
    public class LevelsStaticDataProvider : ILoadableService
    {
        private readonly LocalAddressableService _addressableService;

        private List<LevelStaticData> _levelsData;

        public LevelsStaticDataProvider(LocalAddressableService addressableService)
        {
            _addressableService = addressableService;
        }

        public List<LevelStaticData> GetLevelsData() =>
            _levelsData;

        public LevelStaticData GetDataByLevel(int level) =>
            _levelsData.Find(data => data.Level == level);
        
        public async Task Load()
        {
            _levelsData = await _addressableService.LoadScriptableCollectionFromGroupAsync<LevelStaticData>(AddressableGroupNames
                    .LevelsGroup);
        }

        public LoadingStage LoadingStage =>
            LoadingStage.LoadingGameLevels;
    }
}