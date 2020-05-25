using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ADManager : MonoBehaviour
{
    bool Is_first = true;
    int Min_level_to_show_ad;
    int before_level;
    float _intervaltime;
    float First_interstitial_delay;
    float Interstitial_delay;
    // Start is called before the first frame update

    System.Action<string, MaxSdk.Reward> onRewardRecievedEvent = null;
    void Start()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // AppLovin SDK is initialized, start loading ads
            // MaxSdk.ShowMediationDebugger();
        };


        // MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration config) =>
        // {
        //     // Get value of a variable to use variableValue to alter your business logic
        //     Min_level_to_show_ad = int.Parse(MaxSdk.VariableService.GetString("Min_level_to_show_ad"));
        //     First_interstitial_delay = float.Parse(MaxSdk.VariableService.GetString("First_interstitial_delay"));
        //     Interstitial_delay = float.Parse(MaxSdk.VariableService.GetString("Interstitial_delay"));
        //     Debug.Log("Min_level..." + Min_level_to_show_ad);
        //     Debug.Log("First_interstital..." + First_interstitial_delay);
        //     Debug.Log("Interstitial..." + Interstitial_delay);
        // };   
        // when we test with Lion studios, we will change them.
        Min_level_to_show_ad = 1;
        First_interstitial_delay = 30;
        Interstitial_delay = 30;


        MaxSdk.SetSdkKey("cGEOTbyAl1JSfq8soyo4LfHBhVwuo_3yBFWqBrQqSJ0H9iCetpi6_yQpNBguCD3rTx6iR3hC83PCHk25GU-SzC");
        MaxSdk.InitializeSdk();
        InitializeInterstitialAds();
        // InitializeRewardedAds();
        InitializeBannerAds();

        _intervaltime = First_interstitial_delay;

        //毎秒intervaltime減らす
        this.UpdateAsObservable()
            .Subscribe(_ => _intervaltime -= Time.deltaTime)
            .AddTo(this);
    }

    #region  Interstitial
#if UNITY_IOS
    string interstitialAdUnitId = "e7a7e0a27fcd5715";

#elif UNITY_ANDROID
     string interstitialAdUnitId = "d15b3ee1e39b2354";
#endif
    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;

        // Load the first interstitial
        LoadInterstitial();
    }
    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(interstitialAdUnitId); 
    }
    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
    }
    private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
    {
        // Interstitial ad failed to load. We recommend re-trying in 3 seconds.
        Invoke("LoadInterstitial", 3);
    }
    private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        LoadInterstitial();
    }
    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        // Interstitial ad is hidden. Pre-load the next ad
        LoadInterstitial();
        _intervaltime = Interstitial_delay;
    }

    public void ShowMaxInterstitial(int level)
    {

        if (Is_first)
        {
            before_level = level - 1;
            Is_first = false;
        }

        if (level - before_level >= Min_level_to_show_ad)
        {
            if (_intervaltime < 0 && MaxSdk.IsInterstitialReady(interstitialAdUnitId))
            {
                before_level = level;
                if (GameManager.totalStagesPlayed != 1)
                {
                    MaxSdk.ShowInterstitial(interstitialAdUnitId);
                    Debug.Log("show_ads");
                }
            }
        }
    }
    public void ShowInterstitialForce()
    {
        MaxSdk.ShowInterstitial(interstitialAdUnitId);
    }

    #endregion


//     #region  Reward
// #if UNITY_IOS
//     string rewardedAdUnitId = "0e35d00e6fb57856";

// #elif UNITY_ANDROID
//     string rewardedAdUnitId = "a4e7dca245a24b3f";
// #endif

//     public void InitializeRewardedAds()
//     {
//         // Attach callback
//         MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
//         MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
//         MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
//         MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
//         MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
//         MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
//         MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

//         // Load the first RewardedAd
//         LoadRewardedAd();
//     }

//     private void LoadRewardedAd()
//     {
//         MaxSdk.LoadRewardedAd(rewardedAdUnitId);
//     }

//     private void OnRewardedAdLoadedEvent(string adUnitId)
//     {
//         // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
//     }

//     private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
//     {
//         // Rewarded ad failed to load. We recommend re-trying in 3 seconds.
//         Invoke("LoadRewardedAd", 3);
//     }

//     private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
//     {
//         // Rewarded ad failed to display. We recommend loading the next ad
//         LoadRewardedAd();
//     }

//     private void OnRewardedAdDisplayedEvent(string adUnitId) { }

//     private void OnRewardedAdClickedEvent(string adUnitId) { }

//     private void OnRewardedAdDismissedEvent(string adUnitId)
//     {
//         // Rewarded ad is hidden. Pre-load the next ad
//         LoadRewardedAd();
//     }

//     private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
//     {
//         // Rewarded ad was displayed and user should receive the reward
//         //このなかに見終わったらやりたい処理を書く。
//         onRewardRecievedEvent?.Invoke(adUnitId, reward);
//         onRewardRecievedEvent = null;
//     }

//     public void ShowMaxReward(System.Action<string, MaxSdk.Reward> onRewardRecieved = null)
//     {
//         onRewardRecievedEvent = onRewardRecieved;
//         if (MaxSdk.IsRewardedAdReady(rewardedAdUnitId))
//         {
//             MaxSdk.ShowRewardedAd(rewardedAdUnitId);

//             if (Application.isEditor)
//             {
//                 OnRewardedAdReceivedRewardEvent(rewardedAdUnitId, new MaxSdk.Reward());
//             }
//         }
//     }
//     #endregion


    #region Banner
#if UNITY_IOS
    string bannerAdUnitId = "544b59887f96dc74"; // Retrieve the id from your account

#elif UNITY_ANDROID
    string bannerAdUnitId = "c902bb7f304b8cf8"; // Retrieve the id from your account
#endif
    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
        // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.white);
        MaxSdk.ShowBanner(bannerAdUnitId);
    }
    #endregion
}