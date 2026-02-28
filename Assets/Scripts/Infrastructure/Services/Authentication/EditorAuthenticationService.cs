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
        private readonly ISaveLoadProgressService _saveLoadProgressService;

        public EditorAuthenticationService(StateMachineService stateMachineService, DeviceInfoProvider deviceInfoProvider, ISaveLoadProgressService saveLoadProgressService)
        {
            _stateMachineService = stateMachineService;
            _deviceInfoProvider = deviceInfoProvider;
            _saveLoadProgressService = saveLoadProgressService;
        }
        
        public void SignIn()
        {
            SuccessfullyAuthenticated(_deviceInfoProvider.GetDeviceUniqueId());
        }
        
        private async void SuccessfullyAuthenticated(string userId)
        {
            await _saveLoadProgressService.OnReadyToLoadProgress(userId);
            _stateMachineService.TransitionTo(StateType.LoadingState);
        }
    }
}