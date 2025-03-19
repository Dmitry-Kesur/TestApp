using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public interface IProgressUpdater
    {
        void OnLoadProgress(PlayerProgress playerProgress);
    }
}