using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using MyBox;

public class AdsManager : MonoSingleton<AdsManager>, IUnityAdsListener
{
    string gameId = "4242174";
    string interstitialName = "Interstitial_New_Marble";
    string rewardedName = "Rewarded_Android";
    public event System.Action<bool> OnRewarded;
    [SerializeField] private LevelManager levelManager;
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

        if (Debug.isDebugBuild)
            levelManager?.Pause();

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
        AwardPlayerRewarded(CheckAdResultFailed(showResult));
    }

    private bool CheckAdResultFailed(ShowResult result) => result == ShowResult.Failed;

    private void AwardPlayerRewarded(bool failed)
    {
        OnRewarded?.Invoke(!failed);
    }
}
