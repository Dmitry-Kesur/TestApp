using System.Threading.Tasks;

namespace Infrastructure.Services.Hud
{
    public interface IHudService
    {
        Task ShowHud();
        void HideHud();
        void UpdateHud();
    }
}