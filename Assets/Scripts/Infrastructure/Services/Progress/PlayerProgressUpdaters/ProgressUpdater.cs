namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public class ProgressUpdater : IProgressUpdater
    {
        protected Data.PlayerProgress.Progress progress;

        public void OnLoadProgress(Data.PlayerProgress.Progress progress)
        {
            this.progress = progress;
        }
    }
}