using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.RemoteConfig;
using UnityEngine;

namespace Infrastructure.Services
{
    public class RemoteConfigService : IRemoteConfigService
    {
        public async Task InitializeAsync()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                await FetchRemoteConfig();
                await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        }
        
        public string GetValue(string key) =>
            FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
        
        private async Task FetchRemoteConfig()
        {
            try
            {
                await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
                Debug.Log("Remote Config fetched successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to fetch Remote Config: {ex.Message}");
            }
        }
    }
}