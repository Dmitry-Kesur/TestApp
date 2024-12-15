using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IInitializeAsync
    { 
        Task InitializeAsync();
    }
}