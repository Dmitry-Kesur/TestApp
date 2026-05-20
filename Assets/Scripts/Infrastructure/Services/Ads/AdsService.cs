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

        private string _currentAdsId;
        
        private Action _onAdsShowCompleted;
        private Action _onAdsShowFailed;

        public AdsService(IExceptionLoggerService exceptionLoggerService, IAnalyticsService analyticsService)
        {
            _exceptionLoggerService = exceptionLoggerService;
            _analyticsService = analyticsService;
            InitializeAds();
        }

        public void ShowAds(string adsId, Action completeCallback = null, Action failedCallback = null)
        {
            var adsProvider = GetAdsProviderById(adsId);
            
            if (adsProvider is not { IsLoaded: true } || _currentAdsId != null)
            {
                failedCallback?.Invoke();
                return;
            }
            
            _currentAdsId = adsId;
            
            _onAdsShowCompleted = completeCallback;
            _onAdsShowFailed = failedCallback;

            adsProvider.ShowAds();
        }

        public void HideBanner()
        {
            var adsProvider = GetAdsProviderById(AdsId.Banner) as BannerAdsProvider;
            adsProvider?.Hide();
        }

        public void OnInitializationComplete()
        {
            foreach (var adsProvider in _adsProviders)
            {
                adsProvider.Load();
                adsProvider.OnAdsShowCompleted = OnAdsShowCompleted;
                adsProvider.OnAdsShowFailed = OnAdsShowFailed;
                adsProvider.OnAdsShowStart = OnAdsShowStart;
                adsProvider.OnAdsFailedToLoad = OnAdsFailedToLoad;
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

        private void OnAdsShowCompleted(string adsId)
        {
            if (_currentAdsId != adsId)
            {
                Clear();
                return;
            }
            
            _onAdsShowCompleted?.Invoke();
            Clear();
        }

        private void OnAdsShowStart(string placementId, string adsId) =>
            _analyticsService.LogAdsImpression(placementId, adsId);

        private void OnAdsShowFailed(string placementId)
        {
            _onAdsShowFailed?.Invoke();
            Clear();
            _analyticsService.LogShowAdsFailed(placementId);
        }
        
        private void OnAdsFailedToLoad(string placementId, string errorMessage) =>
            _analyticsService.LogAdsFailedToLoad(placementId, errorMessage);

        private BaseAdsProvider GetAdsProviderById(string adsId) =>
            _adsProviders.Find(ads => ads.AdsId == adsId);

        private void Clear()
        {
            _onAdsShowCompleted = null;
            _onAdsShowFailed = null;

            _currentAdsId = null;
        }
    }
}