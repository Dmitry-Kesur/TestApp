using System;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.PlayerProgressUpdaters
{
    public class SettingsProgressUpdater : IProgressUpdater
    {
        private bool _muteSounds;

        public Action OnProgressChanged { get; set; }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.MuteSounds = _muteSounds;
        }

        public void OnLoadProgress(PlayerProgress playerProgress)
        {
            _muteSounds = playerProgress.MuteSounds;
        }

        public bool MuteSounds =>
            _muteSounds;

        public void ChangeMuteSounds(bool muteSounds)
        {
            _muteSounds = muteSounds;
            OnProgressChanged?.Invoke();
        }
    }
}