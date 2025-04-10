using Infrastructure.Enums;
using Infrastructure.Services.Sound;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class LoseLevelState : State
    {
        private readonly IWindowService _windowService;
        private readonly ISoundService _soundService;
        
        public LoseLevelState(IWindowService windowService, ISoundService soundService)
        {
            _windowService = windowService;
            _soundService = soundService;
        }
        
        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.LoseLevelWindow);
            _soundService.PlaySound(SoundId.Lose);
        }

        public override void Exit()
        {
            _windowService.HideActiveWindow();
        }
    }
}