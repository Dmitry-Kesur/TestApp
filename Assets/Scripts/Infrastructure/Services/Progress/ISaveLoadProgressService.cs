using System.Threading.Tasks;

namespace Infrastructure.Services.Progress
{
    public interface ISaveLoadProgressService
    {
        Task<Data.PlayerProgress.Progress> LoadProgress(string userId);
        void SaveProgress(Data.PlayerProgress.Progress progress);
    }
}