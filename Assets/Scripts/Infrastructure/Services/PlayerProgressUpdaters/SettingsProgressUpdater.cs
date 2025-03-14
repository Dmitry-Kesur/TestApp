using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.PlayerProgressUpdaters
{
    public class SettingsProgressUpdater : IProgressUpdater
    {
        public PlayerProgress Progress { get; set; }

        public void OnLoadProgress(PlayerProgress playerProgress)
        {
            Progress = playerProgress;
        }

        public bool MuteSounds =>
            Progress.MuteSounds;

        public void ChangeMuteSounds(bool muteSounds)
        {
            Progress.MuteSounds = muteSounds;
        }
    }
}