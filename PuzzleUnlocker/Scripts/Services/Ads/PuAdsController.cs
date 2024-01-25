using System;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using Dainty.Ads;
using UnityEngine;

namespace PuzzleUnlocker.Services.Ads
{
    public sealed class PuAdsController : AdsControllerBase, IPuAdsController,
        IBannerAdListener, IInterstitialAdListener, IRewardedVideoAdListener
    {
        private const string APP_KEY =
#if UNITY_ANDROID
            "65d54ef417386c4057d1871a3c76fb37915c17813efc7aac";
#elif UNITY_IOS
#endif

        private const int AD_TYPES = Appodeal.BANNER | Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO;

        public PuAdsController(bool enabled = true) : base(enabled)
        {
        }

        public override void Initialize(bool gdprAccepted)
        {
#if DEV_ADS
            Appodeal.setTesting(true);
#endif
#if DEV_LOG
            Appodeal.setLogLevel(Appodeal.LogLevel.Verbose);
#endif
            Appodeal.muteVideosIfCallsMuted(true);
            Appodeal.setAutoCache(AD_TYPES, true);
            Appodeal.initialize(APP_KEY, AD_TYPES, gdprAccepted);
            Appodeal.setBannerCallbacks(this);
            Appodeal.setInterstitialCallbacks(this);
            Appodeal.setRewardedVideoCallbacks(this);

            _enabled = true;
        }

        public override bool CanShow(AdType type, string placement = null)
        {
            var appodealAdType = GetAppodealAdType(type);
            return placement == null
                ? Appodeal.canShow(appodealAdType)
                : Appodeal.canShow(appodealAdType, placement);
        }

        protected override void ShowInternal(AdType adType, string placement)
        {
            var appodealAdType = GetAppodealAdType(adType);
            if (placement == null)
            {
                Appodeal.show(appodealAdType);
            }
            else
            {
                Appodeal.show(appodealAdType, placement);
            }
        }

        protected override void ShowBannerInternal(BannerPosition position, int offset, string placement)
        {
            if (position != BannerPosition.Bottom || offset != 0)
            {
                throw new NotImplementedException("Only bottom banner without offset implemented :(");
            }

            if (placement == null)
            {
                Appodeal.show(Appodeal.BANNER_BOTTOM);
            }
            else
            {
                Appodeal.show(Appodeal.BANNER_BOTTOM, placement);
            }
        }

        protected override void HideInternal(AdType adType, string placement)
        {
            var appodealAdType = GetAppodealAdType(adType);
            Appodeal.hide(appodealAdType);
            SetBannerState(BannerState.Closed);
        }

        private static int GetAppodealAdType(AdType type)
        {
            return type switch
            {
                AdType.Banner => Appodeal.BANNER,
                AdType.Interstitial => Appodeal.INTERSTITIAL,
                AdType.Rewarded => Appodeal.REWARDED_VIDEO,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        #region Banner callbacks

        public void onBannerLoaded(int height, bool isPrecache)
        {
            DoMainThread(() => SetBannerState(BannerState.Loaded));
        }

        public void onBannerFailedToLoad()
        {
            DoMainThread(() => SetBannerState(BannerState.FailedToLoad));
        }

        public void onBannerShown()
        {
            DoMainThread(() => SetBannerState(BannerState.Shown));
        }

        public void onBannerClicked()
        {
            DoMainThread(() => SetBannerState(BannerState.Clicked));
        }

        public void onBannerExpired()
        {
            DoMainThread(() => SetBannerState(BannerState.Expired));
        }

        #endregion

        #region Interstitial callbacks

        public void onInterstitialLoaded(bool isPrecache)
        {
            DoMainThread(() => SetInterstitialState(InterstitialState.Loaded));
        }

        public void onInterstitialFailedToLoad()
        {
            DoMainThread(() => SetInterstitialState(InterstitialState.FailedToLoad));
        }

        public void onInterstitialShowFailed()
        {
            DoMainThread(() => SetInterstitialState(InterstitialState.FailedToShow));
        }

        public void onInterstitialShown()
        {
            DoMainThread(() => SetInterstitialState(InterstitialState.Shown));
        }

        public void onInterstitialClosed()
        {
            DoMainThread(() => SetInterstitialState(InterstitialState.Closed));
        }

        public void onInterstitialClicked()
        {
            DoMainThread(() => SetInterstitialState(InterstitialState.Clicked));
        }

        public void onInterstitialExpired()
        {
            DoMainThread(() => SetInterstitialState(InterstitialState.Expired));
        }

        #endregion

        #region Rewarded callbacks

        public void onRewardedVideoLoaded(bool precache)
        {
            DoMainThread(() => SetRewardedState(RewardedState.Loaded));
        }

        public void onRewardedVideoFailedToLoad()
        {
            DoMainThread(() => SetRewardedState(RewardedState.FailedToLoad));
        }

        public void onRewardedVideoShowFailed()
        {
            DoMainThread(() => SetRewardedState(RewardedState.FailedToShow));
        }

        public void onRewardedVideoShown()
        {
            DoMainThread(() => SetRewardedState(RewardedState.Shown));
        }

        public void onRewardedVideoFinished(double amount, string name)
        {
            DoMainThread(() => SetRewardedState(RewardedState.Finished));
        }

        public void onRewardedVideoClosed(bool finished)
        {
            DoMainThread(() => SetRewardedState(RewardedState.Closed));
        }

        public void onRewardedVideoExpired()
        {
            DoMainThread(() => SetRewardedState(RewardedState.Expired));
        }

        public void onRewardedVideoClicked()
        {
            DoMainThread(() => SetRewardedState(RewardedState.Clicked));
        }

        #endregion
    }
}