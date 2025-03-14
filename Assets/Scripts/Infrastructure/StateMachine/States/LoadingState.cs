using Infrastructure.Enums;
using Infrastructure.Services;

namespace Infrastructure.StateMachine.States
{
    public class LoadingState : State
    {
        private readonly IWindowService _windowService;
        private readonly IPreloaderService _preloaderService;

        public LoadingState(IWindowService windowService, IPreloaderService preloaderService)
        {
            _windowService = windowService;
            _preloaderService = preloaderService;
        }

        public override async void Enter()
        {
            _windowService.ShowWindow(WindowId.PreloaderWindow);
            await _preloaderService.Load();
        }

        public override void Exit()
        {
            _windowService.HideWindow(WindowId.PreloaderWindow);
        }
    }
}