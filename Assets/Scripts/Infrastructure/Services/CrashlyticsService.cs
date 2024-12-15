using System;
using System.Threading.Tasks;
using Firebase.Crashlytics;
using UnityEngine;

namespace Infrastructure.Services
{
    public class CrashlyticsService : ICrashlyticsService
    {
        public void LogError(string message) =>
            Crashlytics.Log(message);

        public void LogException(Exception exception) =>
            Crashlytics.LogException(exception);

        public async Task InitializeAsync()
        {
            var dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
            Debug.Log($"initializing {nameof(CrashlyticsService)}");
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                
                Crashlytics.ReportUncaughtExceptionsAsFatal = true;
            }
            else
            {
                Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}",dependencyStatus));
            }
        }
    }
}