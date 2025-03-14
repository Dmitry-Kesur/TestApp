using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Infrastructure.Enums;
using Infrastructure.Providers;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class InitializeThirdPartyServicesState : State
    {
        private readonly List<IFirebaseInitialize> _firebaseInitializeServices;

        private readonly RemoteConfigService _remoteConfigService;
        private readonly InAppPurchaseProvider _inAppPurchaseProvider;

        public InitializeThirdPartyServicesState(List<IFirebaseInitialize> firebaseInitializeServices, RemoteConfigService remoteConfigService,
            InAppPurchaseProvider inAppPurchaseProvider)
        {
            _firebaseInitializeServices = firebaseInitializeServices;
            _remoteConfigService = remoteConfigService;
            _inAppPurchaseProvider = inAppPurchaseProvider;
        }

        public override async void Enter()
        {
            try
            {
                await CheckAndFixFirebaseDependencies();
                await InitializeRemoteConfigService();
                InitializeFirebaseServices();
                
                InitializeInAppPurchaseProvider();

                StateMachineService.TransitionTo(StateType.AuthenticationState);
            }
            catch (Exception e)
            {
                Debug.LogError($"[InitializeThirdPartyServicesState] Exception: {e.Message}\n{e.StackTrace}");
            }
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

        private async Task InitializeRemoteConfigService()
        {
            await _remoteConfigService.Initialize();
        }

        private void InitializeInAppPurchaseProvider() =>
            _inAppPurchaseProvider.Initialize();
    }
}