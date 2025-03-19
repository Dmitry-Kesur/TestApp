using Infrastructure.Enums;
using Infrastructure.Providers.Device;
using Infrastructure.Services.Progress;
using Infrastructure.StateMachine;

namespace Infrastructure.Services.Authentication
{
    public class EditorAuthenticationService : IAuthenticationService
    {
        private readonly StateMachineService _stateMachineService;
        private readonly DeviceInfoProvider _deviceInfoProvider;
        private readonly IProgressService _progressService;

        public EditorAuthenticationService(StateMachineService stateMachineService, DeviceInfoProvider deviceInfoProvider, IProgressService progressService)
        {
            _stateMachineService = stateMachineService;
            _deviceInfoProvider = deviceInfoProvider;
            _progressService = progressService;
        }
        
        public void SignIn()
        {
            SuccessfullyAuthenticated(_deviceInfoProvider.GetDeviceUniqueId());
        }
        
        private async void SuccessfullyAuthenticated(string userId)
        {
            await _progressService.LoadPlayerProgress(userId);
            _stateMachineService.TransitionTo(StateType.LoadingState);
        }
    }
}