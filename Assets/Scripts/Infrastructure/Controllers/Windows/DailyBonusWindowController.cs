using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.DailyBonus;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class DailyBonusWindowController : BaseWindowController<DailyBonusWindow>
    {
        private readonly DailyBonusService _dailyBonusService;
        private readonly StateMachineService _stateMachineService;
        private readonly DailyBonusWindowModel _dailyBonusWindowModel;
        
        public DailyBonusWindowController(DailyBonusService dailyBonusService, StateMachineService stateMachineService)
        {
            _dailyBonusService = dailyBonusService;
            _stateMachineService = stateMachineService;
            _dailyBonusService.OnRewardTaken += OnRewardTaken;
            _dailyBonusWindowModel = new DailyBonusWindowModel();
        }

        protected override BaseWindowModel GetModel() => _dailyBonusWindowModel;

        protected override void InitParameters()
        {
            base.InitParameters();
            _dailyBonusWindowModel.DayModels = _dailyBonusService.AvailableDayModels;
        }

        private void OnRewardTaken()
        {
            _dailyBonusService.OnRewardTaken -= OnRewardTaken;
            _stateMachineService.TransitionTo(StateType.MenuState);
        }
    }
}