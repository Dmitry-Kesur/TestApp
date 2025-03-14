using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services
{
    public interface ISaveLoadProgressService
    {
        Task<PlayerProgress> LoadProgress(string userId);
        void SaveProgress(PlayerProgress playerProgress);
    }
}