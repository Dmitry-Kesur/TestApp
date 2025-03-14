using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.PlayerProgressUpdaters
{
    public interface IProgressUpdater
    {
        PlayerProgress Progress { get; set; }
        void OnLoadProgress(PlayerProgress playerProgress);
    }
}