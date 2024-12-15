using System.Threading.Tasks;
using Infrastructure.Data.Preloader;

namespace Infrastructure.Services
{
    public interface ILoadableService
    {
        Task Load();
        LoadingStage LoadingStage { get; }
    }
}