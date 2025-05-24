using System;
using System.Threading.Tasks;
using Firebase.Database;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Log;
using Newtonsoft.Json;

namespace Infrastructure.Services.Progress
{
    public class SaveLoadProgressService : ISaveLoadProgressService, IFirebaseInitialize
    {
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private static readonly string UsersTablePath = "users";
        
        private DatabaseReference _databaseReference;
        
        public SaveLoadProgressService(IExceptionLoggerService exceptionLoggerService)
        {
            _exceptionLoggerService = exceptionLoggerService;
        }

        public void Initialize()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public async Task<Data.PlayerProgress.Progress> LoadProgress(string userId)
        {
            try
            {
                var dataSnapshot = await _databaseReference.Child(UsersTablePath).Child(userId).GetValueAsync();
                if (dataSnapshot.Exists)
                {
                    string jsonData = dataSnapshot.GetRawJsonValue();
                    return JsonConvert.DeserializeObject<Data.PlayerProgress.Progress>(jsonData);
                }
            }
            catch (Exception e)
            {
                _exceptionLoggerService.LogException(e);
                throw;
            }

            return null;
        }

        public async void SaveProgress(Data.PlayerProgress.Progress progress)
        {
            try
            {
                string progressJson = JsonConvert.SerializeObject(progress);
                await _databaseReference.Child(UsersTablePath).Child(progress.UserId)
                    .SetRawJsonValueAsync(progressJson);
            }
            catch (Exception e)
            {
                _exceptionLoggerService.LogException(e);
            }
        }
    }
}