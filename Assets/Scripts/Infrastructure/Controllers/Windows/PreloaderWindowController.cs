using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.Preloader;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class PreloaderWindowController : BaseWindowController<PreloaderWindow>
    {
        private readonly IPreloaderService _preloaderService;
        private readonly PreloaderWindowModel _preloaderWindowModel;
        private readonly StateMachineService _stateMachineService;

        public PreloaderWindowController(IPreloaderService preloaderService, StateMachineService stateMachineService)
        {
            _preloaderService = preloaderService;
            _preloaderService.UpdateLoadingProgressAction = OnUpdateLoadingProgress;

            _stateMachineService = stateMachineService;

            _preloaderWindowModel = new PreloaderWindowModel();
            _preloaderWindowModel.StartGameAction = OnStartGame;
        }

        private void OnStartGame()
        {
            _stateMachineService.TransitionTo(StateType.MenuState);
        }

        public override void OnWindowAdd(BaseWindow view)
        {
            base.OnWindowAdd(view);
            view.SetModel(_preloaderWindowModel);
        }

        private void OnUpdateLoadingProgress(float progress, string stageText)
        {
            _preloaderWindowModel.OnUpdateLoadingProgress(progress, stageText);
        }
    }
}