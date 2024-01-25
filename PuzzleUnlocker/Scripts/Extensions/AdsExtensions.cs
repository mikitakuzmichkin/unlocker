using System;
using Dainty.Ads;
using GameAnalyticsSDK;

public static class AdsExtensions
{
    public static void ShowRewardedOnLevel(this IAdsController adsController, int levelId, string placement,
        Action<bool> onFinished, Action onFailed)
    {
        onFinished += success =>
        {
            if (success)
            {
                GameAnalytics.NewDesignEvent($"Ad:Rewarded:{placement}:{levelId}");
            }
        };

        adsController.ShowRewarded(placement, onFinished, onFailed);
    }

    public static void ShowInterstitialOnLevel(this IAdsController adsController, int levelId, string placement = null,
        Action<bool> onFinished = null)
    {
        adsController.ShowInterstitial(
            placement,
            onFinished,
            () => GameAnalytics.NewDesignEvent($"Ad:Interstitial:{placement}:{levelId}")
        );
    }

    public static void ShowInterstitial(this IAdsController adsController, string placement = null,
        Action<bool> onFinished = null, Action onShow = null)
    {
        if (onFinished == null && onShow == null)
        {
            adsController.ShowInterstitial(placement);
            return;
        }

        adsController.ShowInterstitial(placement, state =>
        {
            // ReSharper disable once RedundantCast
            switch ((InterstitialState) state)
            {
                case InterstitialState.Shown:
                    onShow?.Invoke();
                    break;
                case InterstitialState.Closed:
                    onFinished?.Invoke(true);
                    break;
                case InterstitialState.FailedToLoad:
                case InterstitialState.Expired:
                case InterstitialState.FailedToShow:
                    onFinished?.Invoke(false);
                    break;
            }
        });
    }
}