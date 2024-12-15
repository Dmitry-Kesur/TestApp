using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services
{
    public interface ISaveLoadPlayerProgressService
    {
        Task<PlayerProgress> LoadPlayerProgress(string userId);
        void SavePlayerProgress(PlayerProgress playerProgress);
    }
}