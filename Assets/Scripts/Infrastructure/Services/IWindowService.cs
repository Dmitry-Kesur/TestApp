using Infrastructure.Enums;

namespace Infrastructure.Services
{
    public interface IWindowService
    {
        void ShowWindow(WindowId windowId);
        void HideWindow(WindowId windowId);
        void HideActiveWindow();
    }
}