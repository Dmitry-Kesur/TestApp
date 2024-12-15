using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace Infrastructure.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        public void LogWinLevel(int level) =>
            LogEventParameter("win_level", "level", level.ToString());

        public void LogLoseLevel(int level) =>
            LogEventParameter("lose_level", "level", level.ToString());

        public void LogPurchaseProduct(int productId) =>
            LogEventParameter("buy_product", "product_id", productId.ToString());

        public async Task InitializeAsync()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            Debug.Log($"initializing {nameof(AnalyticsService)}");
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
            }
            else
            {
                Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        }

        private void LogEventParameter(string eventName, string parameterName, string parameterValue) =>
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
    }
}