using System.Threading.Tasks;

namespace Infrastructure.Services.Progress
{
    public interface IProgressService
    {
        Task LoadPlayerProgress(string userId);
        void SavePlayerProgress();
    }
}