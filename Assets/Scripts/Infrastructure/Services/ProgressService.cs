using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;
using Infrastructure.Factories;
using Infrastructure.Services.PlayerProgressUpdaters;

namespace Infrastructure.Services
{
    public class ProgressService : IProgressService
    {
        private readonly ISaveLoadProgressService _saveLoadProgress;
        private readonly List<IProgressUpdater> _progressUpdaters;
        private readonly IProgressFactory _progressFactory;

        private PlayerProgress _playerProgress;

        public ProgressService(ISaveLoadProgressService saveLoadProgress, List<IProgressUpdater> progressUpdaters, IProgressFactory progressFactory)
        {
            _saveLoadProgress = saveLoadProgress;
            _progressUpdaters = progressUpdaters;
            _progressFactory = progressFactory;
        }

        public async Task LoadPlayerProgress(string userId)
        {
            _playerProgress = await _saveLoadProgress.LoadProgress(userId) ?? _progressFactory.CreateNewProgress(userId);

            foreach (var progressUpdater in _progressUpdaters)
                progressUpdater.OnLoadProgress(_playerProgress);
        }

        public void SavePlayerProgress()
        {
            if (_playerProgress == null)
                return;
            
            _saveLoadProgress.SaveProgress(_playerProgress);
        }
    }
}