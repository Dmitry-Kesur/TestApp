using Infrastructure.Services;
using Infrastructure.Services.Hud;
using Infrastructure.Services.Level;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class GameLoopState : State
    {
        private readonly ILevelsService _levelsService;
        private readonly IHudService _hudService;
        private readonly IWindowService _windowService;

        public GameLoopState(ILevelsService levelsService, IHudService hudService, IWindowService windowService)
        {
            _levelsService = levelsService;
            _hudService = hudService;
            _windowService = windowService;
        }

        public override async void Enter()
        {
            await _hudService.ShowHud();
            _windowService.HideActiveWindow();

            if (_levelsService.LevelStarted)
                _levelsService.Resume();
            else
                _levelsService.Start();
        }

        public override void Exit()
        {
            _hudService.HideHud();
            _windowService.HideActiveWindow();
        }
    }
}