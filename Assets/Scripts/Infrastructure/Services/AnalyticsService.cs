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

        public void Initialize()
        {
           Debug.Log("Analytics service initialized");
        }

        private void LogEventParameter(string eventName, string parameterName, string parameterValue) =>
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
    }
}