using System.Threading.Tasks;
using Firebase.Database;
using Infrastructure.Data.PlayerProgress;
using Newtonsoft.Json;

namespace Infrastructure.Services.Progress
{
    public class FirebaseProgressRepository : IProgressRepository
    {
        private const string UsersTablePath = "users";
        
        private DatabaseReference _databaseReference;

        public void Initialize()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public async Task<ProgressData> Load(string userId)
        {
            var snap = await _databaseReference.Child(UsersTablePath).Child(userId).GetValueAsync();
            if (!snap.Exists) return null;

            var json = snap.GetRawJsonValue();
            return string.IsNullOrWhiteSpace(json) ? null : JsonConvert.DeserializeObject<ProgressData>(json);
        }

        public async Task Save(string userId, ProgressData progressData)
        {
            var json = JsonConvert.SerializeObject(progressData);
            await _databaseReference.Child(UsersTablePath).Child(userId).SetRawJsonValueAsync(json);
        }
    }
}