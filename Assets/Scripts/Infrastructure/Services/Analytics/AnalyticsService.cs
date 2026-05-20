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
            LogEventParameter(AnalyticEvent.WinLevel, AnalyticsParams.Level, level.ToString());

        public void LogLoseLevel(int level) =>
            LogEventParameter(AnalyticEvent.LoseLevel, AnalyticsParams.Level, level.ToString());

        public void LogPurchaseInGameProduct(int productId) =>
            LogEventParameter(AnalyticEvent.PurchaseInGameProduct, AnalyticsParams.ProductId, productId.ToString());

        public void LogCompleteInAppPurchaseProduct(string productId)
        {
            FirebaseAnalytics.LogEvent(
                FirebaseAnalytics.EventPurchase,
                new Parameter(FirebaseAnalytics.ParameterItemID, productId)
            );
        }

        public void LogAdsImpression(string placementId, string adsId) =>
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, new Parameter(AnalyticsParams.AdPlatform, placementId),
                new Parameter("ad_format", adsId));

        public void LogShowAdsFailed(string placementId) =>
            LogEventParameter(AnalyticEvent.ShowAdsFailed, AnalyticsParams.AdPlatform, placementId);

        public void LogAdsFailedToLoad(string placementId, string errorMessage) =>
            FirebaseAnalytics.LogEvent(AnalyticEvent.AdsFailedToLoad, new Parameter(AnalyticsParams.AdPlatform, placementId), new Parameter(AnalyticsParams.ErrorMessage, errorMessage));

        public void LogFailedInAppPurchaseProduct(string productId) =>
            LogEventParameter(AnalyticEvent.FailedPurchaseInAppProduct, AnalyticsParams.ProductId, productId);

        public void LogInAppPurchaseProductRestore(string productId) =>
            LogEventParameter(AnalyticEvent.RestorePurchaseInAppProduct, AnalyticsParams.ProductId, productId);

        public void LogReceiveReward(int rewardId) =>
            LogEventParameter(AnalyticEvent.ReceiveReward, AnalyticsParams.RewardId, rewardId.ToString());

        private void LogEventParameter(string eventName, string parameterName, string parameterValue) =>
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
    }
}