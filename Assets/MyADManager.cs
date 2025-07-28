using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class MyADManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

    public string GAME_ID = "5905965";

#if UNITY_ANDROID
    public string REWARDERD_ID = "Rewarded_Android";
    public string INTERSTITIAL_ID = "Interstitial_Android";
    public string BANNER_ID = "Banner_Android";
#endif

    [SerializeField] Button initButton;
    [SerializeField] Button toggleButton;
    [SerializeField] Button loadRewardedButton;
    public Button showRewardedButton;
    [SerializeField] Button loadInterstitialButton;
    [SerializeField] Button showInterstitialButton;

    bool showBanner = false;

    //private void Start()
    //{
    //    initButton.onClick.AddListener(Initialize);
    //    toggleButton.onClick.AddListener(ToggleBanner);
    //    loadRewardedButton.onClick.AddListener(LoadRewardedAd);
    //    showRewardedButton.onClick.AddListener(ShowRewardedAd);
    //    loadInterstitialButton.onClick.AddListener(LoadInterstitialAd);
    //    showInterstitialButton.onClick.AddListener(ShowInterstitialAd);
    //}

    bool succesfully_initialized = false;
    int count = 0;
    bool trying = false;

    IEnumerator Start()
    
    {
        succesfully_initialized = false;
        count = 0;

        toggleButton.onClick.AddListener(ToggleBanner);

        while (count < 5  && !succesfully_initialized)
        {
            trying = true;
            count++;
            Initialize();
            yield return new WaitWhile(() => trying);

            if (!succesfully_initialized)
                yield return new WaitForSeconds(30f);
        }

        if (!succesfully_initialized)
        {
            Debug.Log("Failed to initialize Unity Ads after multiple attempts. Please check your network connection or game ID.");
        }
        else
        {
            showInterstitialButton.onClick.AddListener(ShowInterstitialAd);
            showRewardedButton.onClick.AddListener(ShowRewardedAd);
            showRewardedButton.gameObject.SetActive(false);
        }
    }

    // 1. Initialize Advertisement

    public void Initialize()
    {
        if (Advertisement.isSupported)
        {
            Debug.Log(Application.platform + " supported by Advertisement");
        }
        Advertisement.Initialize(GAME_ID, true, this);
        
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        succesfully_initialized = true;
        trying = false;
        Advertisement.Load(REWARDERD_ID, this);
        Advertisement.Load(INTERSTITIAL_ID, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads initialization failed: {error.ToString()} - {message}");
        trying = false;

    }

    public void ToggleBanner()
    {
        showBanner = !showBanner;

        if (showBanner)
        {
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
            Advertisement.Banner.Show(BANNER_ID);
        }
        else
        {
            Advertisement.Banner.Hide(false);
        }
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(REWARDERD_ID, this);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(REWARDERD_ID, this);
    }

    public void LoadInterstitialAd()
    {
        Advertisement.Load(INTERSTITIAL_ID, this);
    }

    public void ShowInterstitialAd()
    {
        Advertisement.Show(INTERSTITIAL_ID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad loaded successfully for placement: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load ad for placement {placementId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Failed to show ad for placement {placementId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {

    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        bool isRewardedAd = placementId == REWARDERD_ID;
        
        if (isRewardedAd)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.SKIPPED:
                    Debug.Log("Rewarded ad was skipped.");
                    break;
                case UnityAdsShowCompletionState.COMPLETED:
                    Debug.Log("Rewarded ad completed successfully.");
                    Camera.main.GetComponent<swiipeCounter>().swipecount--;
                    Camera.main.GetComponent<swiipeCounter>().whiteBall.swipeCounter--;
                    Camera.main.GetComponent<swiipeCounter>().blackBall.swipeCounter--;
                    Debug.Log(Camera.main.GetComponent<swiipeCounter>().swipecount);
                    // Here you can reward the player
                    break;
                dafault:
                    Debug.LogWarning("Shouldn't happen.");
                    break;
            }
        }

        Advertisement.Load(placementId, this);
    }
}
