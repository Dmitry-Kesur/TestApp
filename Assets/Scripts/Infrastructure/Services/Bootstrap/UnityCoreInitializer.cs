using System.Threading.Tasks;
using Unity.Services.Core;

namespace Infrastructure.Services.Bootstrap
{
    public class UnityCoreInitializer : ICoreThirdPartyInitializable
    {
        public async Task InitializeAsync()
        {
            await UnityServices.InitializeAsync();
        }
    }
}