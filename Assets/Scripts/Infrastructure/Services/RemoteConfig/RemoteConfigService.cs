using System;
using System.Threading.Tasks;
using Firebase.RemoteConfig;
using UnityEngine;

namespace Infrastructure.Services.RemoteConfig
{
    public class RemoteConfigService
    {
        private FirebaseRemoteConfig _remoteConfig;
        
        public async Task Initialize()
        {
            _remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            
            var fetched = await FetchRemoteConfig();
            if (fetched)
            {
                await _remoteConfig.ActivateAsync();
                Debug.Log("RemoteConfig initialized and activated");
            }
            else
            {
                Debug.LogWarning("RemoteConfig fetch failed");
            }
        }

        public string GetValue(string key) =>
            _remoteConfig.GetValue(key).StringValue;

        private async Task<bool> FetchRemoteConfig()
        {
            try
            {
                var fetchTask = _remoteConfig.FetchAsync(TimeSpan.Zero);
                await fetchTask;

                if (fetchTask.IsCanceled)
                {
                    Debug.LogWarning("Fetch canceled");
                    return false;
                }
                if (fetchTask.IsFaulted)
                {
                    Debug.LogError($"Fetch faulted: {fetchTask.Exception}");
                    return false;
                }

                return _remoteConfig.Info.LastFetchStatus == LastFetchStatus.Success;
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception during fetch: {e}");
                return false;
            }
        }
    }
}