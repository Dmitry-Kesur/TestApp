using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Infrastructure.Enums;
using Infrastructure.Services.Bootstrap;
using Unity.Services.Core;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class InitializeThirdPartyServicesState : State
    {
        private readonly List<IThirdPartyInitializable> _firebaseInitializeServices;

        public InitializeThirdPartyServicesState(List<IThirdPartyInitializable> firebaseInitializeServices)
        {
            _firebaseInitializeServices = firebaseInitializeServices;
        }

        public override async void Enter()
        {
            try
            {
                await InitializeServices();

                StateMachineService.TransitionTo(StateType.AuthenticationState);
            }
            catch (Exception e)
            {
                Debug.LogError($"[InitializeThirdPartyServicesState] Exception: {e.Message}\n{e.StackTrace}");
            }
        }

        private async Task InitializeServices()
        {
            await InitAsyncServices();
            InitializeFirebaseServices();
        }

        private async Task InitAsyncServices()
        {
            await UnityServices.InitializeAsync();
            await CheckAndFixFirebaseDependencies();
        }

        private async Task CheckAndFixFirebaseDependencies()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase dependencies are available.");
            }
            else
            {
                Debug.LogError($"Firebase dependencies error: {dependencyStatus}");
                throw new InvalidOperationException("Firebase dependencies are not available.");
            }
        }

        private void InitializeFirebaseServices()
        {
            foreach (var firebaseInitializeService in _firebaseInitializeServices)
            {
                try
                {
                    firebaseInitializeService.Initialize();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to initialize Firebase service {firebaseInitializeService.GetType().Name}: {e.Message}");
                }
            }
        }
    }
}