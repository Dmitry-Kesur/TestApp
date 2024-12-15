using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Data.Ads
{
    public class BaseAdsProvider : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public Action OnAdsShowCompleteAction;

        public virtual string AdsId => "";
        
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("[Ads-Service]: Ads loaded.");
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"[Ads-Service]: Ads failed to load. {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            
        }

        public void OnUnityAdsShowStart(string placementId)
        {
           
        }

        public void OnUnityAdsShowClick(string placementId)
        {
          
        }

        public virtual void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            OnAdsShowCompleteAction?.Invoke();
        }

        public void ShowAds()
        {
            Advertisement.Show(GetPlacementId(), this);
        }

        public void Load()
        {
           Advertisement.Load(GetPlacementId(), this);
        }

        protected virtual string GetAndroidPlacementId() =>
            "";

        protected virtual string GetIOSPlacementId() =>
            "";
        
        private string GetPlacementId() =>
            Application.platform == RuntimePlatform.Android ? GetAndroidPlacementId() : GetIOSPlacementId();
    }
}