using System;
using System.Collections.Generic;
using Infrastructure.Data.Ads;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Services
{
    public class AdsService : IAdsService, IUnityAdsInitializationListener
    {
        private static readonly string AndroidGameId = "5578461";
        private static readonly bool EnabledTestMode = false;
        
        private readonly List<BaseAdsProvider> _adsProviders = new();

        public AdsService()
        {
            InitializeAds();
        }

        public void ShowAds(string adsId)
        {
            var adsProvider = GetAdsProviderById(adsId);
            adsProvider.ShowAds();
        }

        public Action OnShowCompleteAdsAction { get; set; }

        public void OnInitializationComplete()
        {
            foreach (var adsProvider in _adsProviders)
            {
                adsProvider.Load();
                adsProvider.OnAdsShowCompleteAction = OnAdsShowComplete;
            }

            Debug.Log("[Ads-Service]: Ads initialized.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"[Ads-Service]: Ads initialize error. {error.ToString()} - {message}");
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
        }
        
        private void OnAdsShowComplete()
        {
            OnShowCompleteAdsAction?.Invoke();
        }

        private BaseAdsProvider GetAdsProviderById(string adsId)
        {
            var adsProvider = _adsProviders.Find(ads => ads.AdsId == adsId);
            return adsProvider;
        }
    }
}