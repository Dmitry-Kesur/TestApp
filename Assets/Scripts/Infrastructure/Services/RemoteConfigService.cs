using System;
using System.Threading.Tasks;
using Firebase.RemoteConfig;
using UnityEngine;

namespace Infrastructure.Services
{
    public class RemoteConfigService
    {
        public async Task Initialize()
        {
            var fetched = await FetchRemoteConfig();
            if (fetched)
            {
                await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                Debug.Log("RemoteConfig initialized and activated");
            }
            else
            {
                Debug.LogWarning("RemoteConfig fetch failed");
            }
        }

        public string GetValue(string key) =>
            FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;

        private async Task<bool> FetchRemoteConfig()
        {
            try
            {
                var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
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

                return FirebaseRemoteConfig.DefaultInstance.Info.LastFetchStatus == LastFetchStatus.Success;
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception during fetch: {e}");
                return false;
            }
        }
    }
}