using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Level;
using Infrastructure.Data.Preloader;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Preloader;

namespace Infrastructure.Providers.Level
{
    public class LevelsStaticDataProvider : ILoadableService
    {
        public Action OnLevelsDataLoaded;
        
        private readonly LocalAddressableService _addressableService;

        private List<LevelStaticData> _levelsData;

        public LevelsStaticDataProvider(LocalAddressableService addressableService)
        {
            _addressableService = addressableService;
        }

        public int MaxLevel =>
            GetLevelsData().Count;

        public List<LevelStaticData> GetLevelsData()
        {
            _levelsData.Sort((levelA, levelB) => levelA.Level - levelB.Level);
            return _levelsData;
        }

        public LevelStaticData GetDataByLevel(int level) =>
            _levelsData.Find(data => data.Level == level);

        public async Task Load()
        {
            _levelsData = await _addressableService.LoadScriptableCollectionFromGroupAsync<LevelStaticData>(AddressableGroupNames
                    .LevelsGroup);
            
            OnLevelsDataLoaded?.Invoke();
        }

        public LoadingStage LoadingStage =>
            LoadingStage.LoadingGameLevels;
    }
}