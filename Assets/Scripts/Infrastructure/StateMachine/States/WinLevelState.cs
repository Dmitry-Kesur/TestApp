using Infrastructure.Enums;
using Infrastructure.Services;

namespace Infrastructure.StateMachine.States
{
    public class WinLevelState : State
    {
        private readonly IWindowService _windowService;
        private readonly ISoundService _soundService;
        
        public WinLevelState(IWindowService windowService, ISoundService soundService)
        {
            _windowService = windowService;
            _soundService = soundService;
        }
        
        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.WinLevelWindow);
            _soundService.PlaySound(SoundId.Win);
        }
    }
}