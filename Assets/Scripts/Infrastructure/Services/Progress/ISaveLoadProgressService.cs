using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.Progress
{
    public interface ISaveLoadProgressService
    {
        Task<PlayerProgress> LoadProgress(string userId);
        void SaveProgress(PlayerProgress playerProgress);
    }
}