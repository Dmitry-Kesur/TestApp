using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Enums;
using Infrastructure.Services.Bootstrap;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class InitializeThirdPartyServicesState : State
    {
        private readonly List<ICoreThirdPartyInitializable> _coreThirdPartyServices;
        private readonly List<IThirdPartyInitializable> _thirdPartyServices;

        public InitializeThirdPartyServicesState(List<ICoreThirdPartyInitializable> coreThirdPartyServices, List<IThirdPartyInitializable> thirdPartyServices)
        {
            _coreThirdPartyServices = coreThirdPartyServices;
            _thirdPartyServices = thirdPartyServices;
        }

        public override async void Enter()
        {
            try
            {
                await InitializeCoreAsync();
                InitializeServices();

                StateMachineService.TransitionTo(StateType.AuthenticationState);
            }
            catch (Exception e)
            {
                Debug.LogError($"[InitializeThirdPartyServicesState] Exception: {e.Message}\n{e.StackTrace}");
            }
        }
        
        private async Task InitializeCoreAsync()
        {
            await Task.WhenAll(
                _coreThirdPartyServices.Select(x => x.InitializeAsync())
            );
        }

        private void InitializeServices()
        {
            foreach (var thirdPartyService in _thirdPartyServices)
            {
                try
                {
                    thirdPartyService.Initialize();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to initialize service {thirdPartyService.GetType().Name}: {e.Message}");
                }
            }
        }
    }
}