using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using MyBox;

public class AdsManager : MonoSingleton<AdsManager>, IUnityAdsListener
{
#if UNITY_ANDROID
    string gameId = "4242174";
    string interstitialName = "Interstitial_New_Marble";
    string rewardedName = "Rewarded_Android";
    public event System.Action onRewarded;
#endif
    protected override void OnAwake() 
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Advertisement.Initialize(gameId);
        Advertisement.AddListener(this);
    }

    public void PlayInterstitial() 
    {
        if (Advertisement.IsReady(interstitialName) && !Debug.isDebugBuild)
            Advertisement.Show(interstitialName);

#if UNITY_EDITOR
        if (Advertisement.IsReady(interstitialName))
            Advertisement.Show(interstitialName);
#endif
    }
    public void PlayVideoReward() 
    {
        if (Advertisement.IsReady(rewardedName) && !Debug.isDebugBuild)
            Advertisement.Show(rewardedName);
#if UNITY_EDITOR
        if (Advertisement.IsReady(rewardedName))
            Advertisement.Show(rewardedName);
#endif
    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == rewardedName && showResult == ShowResult.Finished)
            AwardPlayerByVideoRewarded();
        else if (placementId == interstitialName && showResult == ShowResult.Finished)
            AwardPlayerByInterstitial();
    }

    private void AwardPlayerByInterstitial()
    { 
        
    }

    private void AwardPlayerByVideoRewarded()
    {
        onRewarded?.Invoke();
    }
}
