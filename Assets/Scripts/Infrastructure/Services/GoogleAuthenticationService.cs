using System.Threading.Tasks;
using Google;
using Infrastructure.Enums;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services
{
    public class GoogleAuthenticationService : IAuthenticationService, IInitializable
    {
        private static readonly string WebClientId = "687813169709-hmu5qv0mpekcaq7hq8sbjft7u8k2iut0.apps.googleusercontent.com";
        private static readonly string TestEditorUserId = "687813169709";
        
        private readonly StateMachineService _stateMachineService;
        private readonly IPlayerProgressService _playerProgressService;
        
        private GoogleSignInConfiguration _configuration;

        public GoogleAuthenticationService(StateMachineService stateMachineService, IPlayerProgressService playerProgressService)
        {
            _stateMachineService = stateMachineService;
            _playerProgressService = playerProgressService;
        }

        public void Initialize()
        {
            #if !UNITY_EDITOR
            CreateConfiguration();
            #endif
        }

        public void SignIn()
        {
            #if !UNITY_EDITOR
            GoogleSignIn.Configuration = _configuration;
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.FromCurrentSynchronizationContext());
            #else
            SuccessfullyAuthenticated(TestEditorUserId);
            #endif
        }

        private void CreateConfiguration()
        {
            _configuration = new GoogleSignInConfiguration()
            {
                WebClientId = WebClientId,
                RequestIdToken = true
            };
        }

        private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
        {
            if (task.IsFaulted)
            {
                Debug.LogError("SignIn Failed: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                GoogleSignInUser user = task.Result;
                SuccessfullyAuthenticated(user.UserId);
            }
        }

        private async void SuccessfullyAuthenticated(string userId)
        {
            await _playerProgressService.LoadPlayerProgress(userId);
            _stateMachineService.TransitionTo(StateType.LoadingState);
        }
    }
}