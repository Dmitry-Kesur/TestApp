using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;
using Infrastructure.Services.PlayerProgressUpdaters;

namespace Infrastructure.Services
{
    public class PlayerProgressService : IPlayerProgressService
    {
        private readonly ISaveLoadPlayerProgressService _saveLoadPlayerProgressService;
        private readonly List<IProgressUpdater> _progressUpdaters;

        private PlayerProgress _playerProgress;

        public PlayerProgressService(ISaveLoadPlayerProgressService saveLoadPlayerProgressService, List<IProgressUpdater> progressUpdaters)
        {
            _saveLoadPlayerProgressService = saveLoadPlayerProgressService;
            _progressUpdaters = progressUpdaters;

            SubscribeListenersProgressUpdaters();
        }

        public async Task LoadPlayerProgress(string userId)
        {
            _playerProgress = await _saveLoadPlayerProgressService.LoadPlayerProgress(userId);

            foreach (var progressUpdater in _progressUpdaters)
                progressUpdater.OnLoadProgress(_playerProgress);
        }

        public void SavePlayerProgress()
        {
            if (_playerProgress == null)
                return;
            
            _saveLoadPlayerProgressService.SavePlayerProgress(_playerProgress);
        }

        private void SubscribeListenersProgressUpdaters()
        {
            foreach (var progressUpdater in _progressUpdaters)
                progressUpdater.OnProgressChanged = () => OnProgressChanged(progressUpdater);
        }

        private void OnProgressChanged(IProgressUpdater progressUpdater) =>
            progressUpdater.UpdateProgress(_playerProgress);
    }
}