using Infrastructure.Enums;
using Infrastructure.StateMachine;

namespace Infrastructure.Services.Level
{
    public class LevelFlowService
    {
        private readonly ILevelsService _levelsService;
        private readonly StateMachineService _stateMachineService;

        public LevelFlowService(ILevelsService levelsService, StateMachineService stateMachineService)
        {
            _levelsService = levelsService;
            _stateMachineService = stateMachineService;
        }

        public void StartLevel()
        {
            _levelsService.StartLevel();
            _stateMachineService.TransitionTo(StateType.GameLoopState);
        }

        public void RestartLevel()
        {
            _levelsService.Restart();
            _stateMachineService.TransitionTo(StateType.GameLoopState);
        }

        public void ReviveLevel()
        {
            _levelsService.Revive();
            _stateMachineService.TransitionTo(StateType.GameLoopState);
        }

        public void ResumeLevel()
        {
            _stateMachineService.TransitionTo(StateType.GameLoopState);
            _levelsService.Resume();
        }

        public void PauseLevel()
        {
            _levelsService.Pause();
            _stateMachineService.TransitionTo(StateType.PauseGameLoopState);
        }
        
        public void BackToMenu()
        {
            _levelsService.Stop();
            _stateMachineService.TransitionTo(StateType.MenuState);
        }
    }
}