using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using MyBox;

public class AdManager : MonoSingleton<AdManager>
{
    private InterstitialManager m_interstitialManager;
    private RewardedManager m_rewardedManager;

    protected override void OnAwake() 
    {
        // override the awake method, so use this like Awake method
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        m_interstitialManager = new InterstitialManager();
        m_rewardedManager = new RewardedManager();
    }

    public void RequestWatchInterstitial(System.Action r) 
    {
        m_interstitialManager.OnInterstitialSucessfulShown += delegate { r.Invoke(); };
        m_interstitialManager.CreateInsterstitial().LoadAdInterstitial().ShowInterstitial();
    }

    public void RequestWatchRewarded(System.Action<string,double> r)
    {
        m_rewardedManager.OnRewardedSucessfulShown += r;
        m_rewardedManager.CreateRewarded().LoadAdRewarded().ShowRewarded();
    }

    private class InterstitialManager
    {
        private InterstitialAd interstitial;
        public event System.Action OnInterstitialSucessfulShown;
        event System.Action OnInterstitialFailShown;

        public InterstitialManager CreateInsterstitial()
        {
            string unitId = "";

#if UNITY_ANDROID
            if (Debug.isDebugBuild)
            // for testing not ia an oficial Id;
                unitId = "ca-app-pub-3940256099942544/1033173712";
            else
                unitId = "ca-app-pub-7111056526475752/7014021094";
#else
        string unitId = "unexpected_platform";
#endif
            this.interstitial = new InterstitialAd(unitId);
            return this;
        }
        public InterstitialManager LoadAdInterstitial()
        {
            AdRequest request = new AdRequest.Builder().Build();
            this.interstitial.LoadAd(request);
            return this;
        }
        public InterstitialManager ShowInterstitial()
        {
            if (this.interstitial.IsLoaded())
                this.interstitial.Show();
            this.interstitial.OnAdClosed += HandleInterstitialClosed;
            return this;
        }
        public void HandleInterstitialClosed(object sender, System.EventArgs args)
        {
            OnInterstitialSucessfulShown?.Invoke();
            DeleteInterstitialInstance();
        }
        private void DeleteInterstitialInstance() 
        {
            OnInterstitialSucessfulShown = null;
            this.interstitial.Destroy();
        }
    }

    private class RewardedManager
    {
        private RewardedAd rewardedAd;
        public event System.Action<string,double> OnRewardedSucessfulShown;
        event System.Action OnRewardedFailShown;

        public RewardedManager CreateRewarded()
        {
            string unitId = "";

#if UNITY_ANDROID
            if (Debug.isDebugBuild)
            // for testing not ia an oficial Id;
                unitId = "ca-app-pub-3940256099942544/5224354917";
            else
                unitId = "ca-app-pub-7111056526475752/8149819423";
#else
        string unitId = "unexpected_platform";
#endif
            this.rewardedAd = new RewardedAd(unitId);
            return this;
        }
        public RewardedManager LoadAdRewarded()
        {
            AdRequest request = new AdRequest.Builder().Build();
            this.rewardedAd.LoadAd(request);
            return this;
        }
        public RewardedManager ShowRewarded()
        {
            if (this.rewardedAd.IsLoaded())
                this.rewardedAd.Show();
            this.rewardedAd.OnUserEarnedReward += HandleRewarded;
            return this;
        }
        private void HandleRewarded(object sender, Reward args)
        {
            OnRewardedSucessfulShown?.Invoke(args.Type,args.Amount);
            DeleteRewardedInstance();
        }
        private void DeleteRewardedInstance()
        {
            OnRewardedSucessfulShown = null;
            this.rewardedAd.Destroy();
        }
    }
}
