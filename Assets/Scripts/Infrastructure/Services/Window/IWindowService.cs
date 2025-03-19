using Infrastructure.Enums;

namespace Infrastructure.Services.Window
{
    public interface IWindowService
    {
        void ShowWindow(WindowId windowId);
        void HideWindow(WindowId windowId);
        void HideActiveWindow();
    }
}