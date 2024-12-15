using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Infrastructure.Data.PlayerProgress;
using Newtonsoft.Json;
using UnityEngine;

namespace Infrastructure.Services
{
    public class SaveLoadPlayerProgressService : ISaveLoadPlayerProgressService, IInitializeAsync
    {
        private DatabaseReference _databaseReference;

        public async Task InitializeAsync()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            Debug.Log($"initializing {nameof(SaveLoadPlayerProgressService)}");

            if (dependencyStatus == DependencyStatus.Available)
                _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            else
                Debug.LogError($"Could not resolve all Firebase dependencies");
        }

        public async Task<PlayerProgress> LoadPlayerProgress(string userId)
        {
            var dataSnapshot = await _databaseReference.Child("users").Child(userId).GetValueAsync();
            if (dataSnapshot.Exists)
            {
                string jsonData = dataSnapshot.GetRawJsonValue();
                return JsonConvert.DeserializeObject<PlayerProgress>(jsonData);
            }

            return CreateNewPlayerProgress(userId);
        }

        public async void SavePlayerProgress(PlayerProgress playerProgress)
        {
            string progressJson = JsonConvert.SerializeObject(playerProgress);
            await _databaseReference.Child("users").Child(playerProgress.UserId).SetRawJsonValueAsync(progressJson);
        }

        private PlayerProgress CreateNewPlayerProgress(string userId)
        {
            var playerProgress = new PlayerProgress
            {
                ActiveLevel = 1,
                UserId = userId,
                CompleteLevelIds = new List<int>(),
                UnlockedLevelItemIds = new List<int>(),
                PurchasedProductIds = new List<int>()
            };

            return playerProgress;
        }
    }
}