﻿using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;

namespace Infrastructure.Services.Progress
{
    public class SaveLoadProgressService : ISaveLoadProgressService, IFirebaseInitialize
    {
        private static readonly string UsersTablePath = "users";
        
        private DatabaseReference _databaseReference;
        
        public void Initialize()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public async Task<Data.PlayerProgress.Progress> LoadProgress(string userId)
        {
            var dataSnapshot = await _databaseReference.Child(UsersTablePath).Child(userId).GetValueAsync();
            if (dataSnapshot.Exists)
            {
                string jsonData = dataSnapshot.GetRawJsonValue();
                return JsonConvert.DeserializeObject<Data.PlayerProgress.Progress>(jsonData);
            }

            return null;
        }

        public async void SaveProgress(Data.PlayerProgress.Progress progress)
        {
            string progressJson = JsonConvert.SerializeObject(progress);
            await _databaseReference.Child(UsersTablePath).Child(progress.UserId).SetRawJsonValueAsync(progressJson);
        }
    }
}