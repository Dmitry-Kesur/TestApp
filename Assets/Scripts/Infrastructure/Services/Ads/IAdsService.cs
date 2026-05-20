using System;

namespace Infrastructure.Services.Ads
{
    public interface IAdsService
    {
        void ShowAds(string adsId, Action completeCallback = null, Action failedCallback = null);
        void HideBanner();
    }
}