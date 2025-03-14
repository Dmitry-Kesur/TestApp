using System.Threading.Tasks;
using Firebase.Database;
using Infrastructure.Data.PlayerProgress;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class SaveLoadProgressService : ISaveLoadProgressService, IFirebaseInitialize
    {
        private static readonly string UsersTablePath = "users";
        
        private DatabaseReference _databaseReference;
        
        public void Initialize()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public async Task<PlayerProgress> LoadProgress(string userId)
        {
            var dataSnapshot = await _databaseReference.Child(UsersTablePath).Child(userId).GetValueAsync();
            if (dataSnapshot.Exists)
            {
                string jsonData = dataSnapshot.GetRawJsonValue();
                return JsonConvert.DeserializeObject<PlayerProgress>(jsonData);
            }

            return null;
        }

        public async void SaveProgress(PlayerProgress playerProgress)
        {
            string progressJson = JsonConvert.SerializeObject(playerProgress);
            await _databaseReference.Child(UsersTablePath).Child(playerProgress.UserId).SetRawJsonValueAsync(progressJson);
        }
    }
}