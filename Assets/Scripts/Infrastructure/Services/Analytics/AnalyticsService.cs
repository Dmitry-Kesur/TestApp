using Firebase.Analytics;
using Infrastructure.Constants;
using Infrastructure.Enums;
using Infrastructure.Services.Bootstrap;
using UnityEngine;

namespace Infrastructure.Services.Analytics
{
    public class AnalyticsService : IAnalyticsService, IThirdPartyInitializable
    {
        public void Initialize()
        {
            Debug.Log("Analytics service initialized");
        }

        public void LogWinLevel(int level) =>
            LogEventParameter(AnalyticEvent.WinLevel, "level", level.ToString());

        public void LogLoseLevel(int level) =>
            LogEventParameter(AnalyticEvent.LoseLevel, "level", level.ToString());

        public void LogPurchaseInGameProduct(int productId) =>
            LogEventParameter(AnalyticEvent.PurchaseInGameProduct, "product_id", productId.ToString());

        public void LogCompleteInAppPurchaseProduct(string productId)
        {
            FirebaseAnalytics.LogEvent(
                FirebaseAnalytics.EventPurchase,
                new Parameter(FirebaseAnalytics.ParameterItemID, productId)
            );
        }

        public void LogAdsImpression(string placementId, string adsId)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, new Parameter("ad_platform", placementId),
                new Parameter("ad_format", adsId));
        }

        public void LogFailedInAppPurchaseProduct(string productId) =>
            LogEventParameter(AnalyticEvent.FailedPurchaseInAppProduct, "product_id", productId);

        public void LogInAppPurchaseProductRestore(string productId) =>
            LogEventParameter(AnalyticEvent.RestorePurchaseInAppProduct, "product_id", productId);

        public void LogReceiveReward(int rewardId) =>
            LogEventParameter(AnalyticEvent.ReceiveReward, "reward_id", rewardId.ToString());

        private void LogEventParameter(string eventName, string parameterName, string parameterValue) =>
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
    }
}