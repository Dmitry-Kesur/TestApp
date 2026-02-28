using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Data.Ads
{
    public abstract class BaseAdsProvider : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public Action<string, string> OnAdsShowStartAction;
        public Action<string> OnAdsShowCompleted;

        public abstract string AdsId { get; }
        
        public void OnUnityAdsAdLoaded(string placementId) =>
            Debug.Log("[Ads-Service]: Ads loaded.");

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
            Debug.Log($"[Ads-Service]: Ads failed to load. {error.ToString()} - {message}");

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            
        }

        public void OnUnityAdsShowStart(string placementId) =>
            OnAdsShowStartAction?.Invoke(placementId, AdsId);  

        public void OnUnityAdsShowClick(string placementId)
        {
          
        }

        public virtual void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                OnAdsShowCompleted?.Invoke(AdsId);
            }
        }

        public virtual void ShowAds() =>
            Advertisement.Show(GetPlacementId(), this);

        public virtual void Load() =>
            Advertisement.Load(GetPlacementId(), this);

        protected abstract string GetAndroidPlacementId();

        protected abstract string GetIOSPlacementId();
        
        protected string GetPlacementId()
        {
            #if UNITY_ANDROID
                        return GetAndroidPlacementId();
            #elif UNITY_IOS
                return GetIOSPlacementId();
            #else
                return GetAndroidPlacementId();
            #endif
        }
    }
}