using System;
using System.Threading.Tasks;
using Firebase;
using UnityEngine;

namespace Infrastructure.Services.Bootstrap
{
    public class FirebaseCoreInitializer : ICoreThirdPartyInitializable
    {
        public async Task InitializeAsync()
        {
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
    }
}