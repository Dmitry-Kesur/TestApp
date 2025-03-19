namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public class SettingsProgressUpdater : ProgressUpdater
    {
        public bool MuteSounds =>
            progress.MuteSounds;

        public void ChangeMuteSounds(bool muteSounds)
        {
            progress.MuteSounds = muteSounds;
        }
    }
}