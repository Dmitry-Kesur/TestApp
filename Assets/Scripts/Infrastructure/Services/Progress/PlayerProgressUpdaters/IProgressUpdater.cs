namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public interface IProgressUpdater
    {
        void OnLoadProgress(Data.PlayerProgress.Progress progress);
    }
}