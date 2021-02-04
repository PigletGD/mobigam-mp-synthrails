using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public string GameID
    {
        get
        {
#if UNITY_ANDROID
            return "3998533";
#elif UNITY_IOS
            return "3998532";
#endif
        }
    }

    public const string SampleInterstitialAd = "video";
    public const string SampleRewardedAd = "rewardedVideo";
    public const string SampleBannerAd = "bannerAd";

    public event EventHandler<AdFinishEventArgs> OnAdDone;

    public static AdsManager Instance { get; private set; }

    public bool showAds = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Advertisement.Initialize(GameID, true);
            Advertisement.AddListener(this);

            ShowBannerAd();

            showAds = true;

            Debug.Log("Ad Manager Instantiated");
        }
        else Destroy(gameObject);
    }

    public void ShowInterstitialAd()
    {
        ConfigMenu configMenu = FindObjectOfType<ConfigMenu>();

        if (Advertisement.IsReady(SampleInterstitialAd))
        {
            if (configMenu != null)
                configMenu.playAdText.text = "Play Video Ad";

            Advertisement.Show(SampleInterstitialAd);
        }
        else
        {
            if (configMenu != null)
                configMenu.playAdText.text = "No Ads Found";

            Debug.Log("No Interstitial Ads");
        }
        
    }

    IEnumerator ShowBannerAd_Routine()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }

        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);

        Advertisement.Banner.Show(SampleBannerAd);
    }

    public void ShowBannerAd()
    {
        StartCoroutine(ShowBannerAd_Routine());
    }

    public void HideBannerAd()
    {
        if (Advertisement.Banner.isLoaded)
        {
            Advertisement.Banner.Hide();
        }
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady(SampleRewardedAd))
            Advertisement.Show(SampleRewardedAd);
        else
        {
            GameHUD gameHUD = FindObjectOfType<GameHUD>();
            if (gameHUD != null)
            {
                gameHUD.playAdPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Error: Could not load Ads. Try again?";
            }
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (OnAdDone != null)
        {
            AdFinishEventArgs args = new AdFinishEventArgs(placementId, showResult);

            OnAdDone(this, args);
        }
    }
}