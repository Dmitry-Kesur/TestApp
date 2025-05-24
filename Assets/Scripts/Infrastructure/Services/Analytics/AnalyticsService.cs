using Firebase.Analytics;
using Infrastructure.Enums;
using UnityEngine;

namespace Infrastructure.Services.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        public void LogWinLevel(int level) =>
            LogEventParameter("win_level", "level", level.ToString());

        public void LogLoseLevel(int level) =>
            LogEventParameter("lose_level", "level", level.ToString());

        public void LogPurchaseProduct(int productId) =>
            LogEventParameter("buy_in_game_product", "product_id", productId.ToString());

        public void LogCompleteInAppPurchaseProduct(string productId) =>
            LogEventParameter("complete_purchase_in_app_product", "product_id", productId);
        
        public void LogFailedInAppPurchaseProduct(string productId) =>
            LogEventParameter("failed_purchase_in_app_product", "product_id", productId);

        public void LogInAppPurchaseProductRestore(string productId) =>
            LogEventParameter("restore_purchase_in_app_product", "product_id", productId);

        public void LogReceiveReward(RewardType rewardType) =>
            LogEventParameter("receive_reward", "reward_type", rewardType.ToString());

        public void Initialize()
        {
           Debug.Log("Analytics service initialized");
        }

        private void LogEventParameter(string eventName, string parameterName, string parameterValue) =>
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
    }
}