using System;
using System.Collections.Generic;
using Infrastructure.Constants;
using Infrastructure.Data.Ads;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Log;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsInitializationListener
    {
        private const string AndroidGameId = "5578461";
        private const bool EnabledTestMode = true;

        private readonly List<BaseAdsProvider> _adsProviders = new();

        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly IAnalyticsService _analyticsService;

        public AdsService(IExceptionLoggerService exceptionLoggerService, IAnalyticsService analyticsService)
        {
            _exceptionLoggerService = exceptionLoggerService;
            _analyticsService = analyticsService;
            InitializeAds();
        }

        public void ShowAds(string adsId)
        {
            var adsProvider = GetAdsProviderById(adsId);
            adsProvider.ShowAds();
        }

        public void HideBanner()
        {
            var adsProvider = GetAdsProviderById(AdsId.Banner) as BannerAdsProvider;
            adsProvider?.Hide();
        }

        public Action<string> OnAdsShowCompletedAction { get; set; }

        public void OnInitializationComplete()
        {
            foreach (var adsProvider in _adsProviders)
            {
                adsProvider.Load();
                adsProvider.OnAdsShowCompleted = OnAdsShowCompleted;
                adsProvider.OnAdsShowStartAction = OnAdsShowStart;
            }

            Debug.Log("[Ads-Service]: Ads initialized.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            var errorMessage = $"[Ads-Service]: Ads initialize error. {error.ToString()} - {message}";
            _exceptionLoggerService.LogError(errorMessage);
        }
        
        private void InitializeAds()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                CreateAdsProviders();
            
                Advertisement.Initialize(AndroidGameId, EnabledTestMode, this);
            }      
#endif
        }

        private void CreateAdsProviders()
        {
            _adsProviders.Add(new RewardedAdsProvider());
            _adsProviders.Add(new BannerAdsProvider());
        }
        
        private void OnAdsShowCompleted(string adsId) =>
            OnAdsShowCompletedAction?.Invoke(adsId);

        private void OnAdsShowStart(string placementId, string adsId) =>
            _analyticsService.LogAdsImpression(placementId, adsId);

        private BaseAdsProvider GetAdsProviderById(string adsId) =>
            _adsProviders.Find(ads => ads.AdsId == adsId);
    }
}