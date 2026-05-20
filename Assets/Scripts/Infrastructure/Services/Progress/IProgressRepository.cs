using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.Progress
{
    public interface IProgressRepository
    {
        Task<ProgressData> Load(string userId);
        Task Save(string userId, ProgressData progressData);
    }
}