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
        
        private readonly StateMachineService _stateMachineService;
        private readonly IProgressService _progressService;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        private GoogleSignInConfiguration _configuration;

        public GoogleAuthenticationService(StateMachineService stateMachineService, IProgressService progressService, IExceptionLoggerService exceptionLoggerService)
        {
            _stateMachineService = stateMachineService;
            _progressService = progressService;
            _exceptionLoggerService = exceptionLoggerService;
        }

        public void Initialize()
        {
            CreateConfiguration();
        }

        public void SignIn()
        {
            GoogleSignIn.Configuration = _configuration;
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.FromCurrentSynchronizationContext());
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
                var taskException = task.Exception;
                var exceptionMessage = "SignIn Failed: " + taskException;
                Debug.Log(exceptionMessage);
                _exceptionLoggerService.LogException(taskException);
            }
            else if (task.IsCompleted)
            {
                GoogleSignInUser user = task.Result;
                SuccessfullyAuthenticated(user.UserId);
            }
        }

        private async void SuccessfullyAuthenticated(string userId)
        {
            await _progressService.LoadPlayerProgress(userId);
            _stateMachineService.TransitionTo(StateType.LoadingState);
        }
    }
}