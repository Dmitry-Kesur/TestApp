using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Factories.Purchase;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;

namespace Infrastructure.Services.Progress
{
    public class ProgressService : IProgressService
    {
        private readonly ISaveLoadProgressService _saveLoadProgress;
        private readonly List<IProgressUpdater> _progressUpdaters;
        private readonly IProgressFactory _progressFactory;

        private Data.PlayerProgress.Progress _progress;

        public ProgressService(ISaveLoadProgressService saveLoadProgress, List<IProgressUpdater> progressUpdaters, IProgressFactory progressFactory)
        {
            _saveLoadProgress = saveLoadProgress;
            _progressUpdaters = progressUpdaters;
            _progressFactory = progressFactory;
        }

        public async Task LoadPlayerProgress(string userId)
        {
            _progress = await _saveLoadProgress.LoadProgress(userId) ?? _progressFactory.CreateNewProgress(userId);

            foreach (var progressUpdater in _progressUpdaters)
                progressUpdater.OnLoadProgress(_progress);
        }

        public void SavePlayerProgress()
        {
            if (_progress == null)
                return;
            
            _saveLoadProgress.SaveProgress(_progress);
        }
    }
}