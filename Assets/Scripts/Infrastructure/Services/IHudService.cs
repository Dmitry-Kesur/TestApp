using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IHudService
    {
        Task ShowHud();
        void HideHud();
        void UpdateHud();
    }
}