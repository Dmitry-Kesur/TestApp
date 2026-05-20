using System.Threading.Tasks;

namespace Infrastructure.Services.Bootstrap
{
    public interface ICoreThirdPartyInitializable
    {
        Task InitializeAsync();
    }
}