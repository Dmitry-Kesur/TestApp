using Infrastructure.Enums;
using Infrastructure.Services.Level;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class PauseGameLoopState : State
    {
        private readonly IWindowService _windowService;
        private readonly ILevelsService _levelsService;

        public PauseGameLoopState(IWindowService windowService, ILevelsService levelsService)
        {
            _windowService = windowService;
            _levelsService = levelsService;
        }

        public override void Enter()
        {
            _levelsService.Pause();
            _windowService.ShowWindow(WindowId.PauseGameWindow);
        }

        public override void Exit()
        {
            _windowService.HideActiveWindow();
        }
    }
}