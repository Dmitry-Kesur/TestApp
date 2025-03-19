using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public class ProgressUpdater : IProgressUpdater
    {
        protected PlayerProgress progress;

        public void OnLoadProgress(PlayerProgress progress)
        {
            this.progress = progress;
        }
    }
}