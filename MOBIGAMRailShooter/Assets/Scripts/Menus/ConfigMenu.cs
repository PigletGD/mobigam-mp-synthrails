using UnityEngine;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour
{
    public Text intervalTime = null;
    public Slider intervalSlider = null;
    public InputField currencyInputField = null;
    public Text invincibility = null;
    public Text playAdText = null;
    public Text toggleAds = null;

    public void OnEnable()
    {
        if (SaveManager.Instance.state.invincibilityOn)
            invincibility.text = "Disable Invincibility";
        else invincibility.text = "Enable Invincibility";

        if (AdsManager.Instance.showAds)
            toggleAds.text = "Disable Ads";
        else toggleAds.text = "Enable Ads";

        int oldValue = SaveManager.Instance.state.notificationInterval;
        intervalSlider.value = oldValue;
        intervalTime.text = oldValue.ToString() + " mins";
    }

    public void GenerateNotification()
    {
        NotificationHandler.Instance.SendSimpleNotif("Notification", "This notification was sent after 10 seconds", 10f);
    }

    public void SetNotificationIntervalTime(System.Single num)
    {
        int newValue = (int)intervalSlider.value;

        intervalTime.text = newValue.ToString() + " mins";
        SaveManager.Instance.state.notificationInterval = newValue;

        NotificationHandler.Instance.ResetRepeatScheduleNotif();
    }

    public void UnlockAllLevels()
    {
        SaveManager.Instance.state.unlockedLevelTwo = true;
        SaveManager.Instance.state.unlockedLevelThree = true;
    }

    public void AddCurrency() => SaveManager.Instance.state.currency += int.Parse(currencyInputField.text);

    public void Invincibility()
    {
        if (SaveManager.Instance.state.invincibilityOn)
        {
            SaveManager.Instance.state.invincibilityOn = false;
            invincibility.text = "Enable Invincibility";
        }
        else
        {
            SaveManager.Instance.state.invincibilityOn = true;
            invincibility.text = "Disable Invincibility";
        }
    }

    public void PlayVideoAd()
    {
        if (AdsManager.Instance.showAds)
        {
            playAdText.text = "Play Video Ad";
            AdsManager.Instance.ShowInterstitialAd();
        }
        else playAdText.text = "Can't Play Ad";
    }

    public void ToggleAds()
    {
        if (AdsManager.Instance.showAds)
        {
            AdsManager.Instance.showAds = false;
            AdsManager.Instance.HideBannerAd();
            toggleAds.text = "Enable Ads";
        }
        else
        {
            AdsManager.Instance.showAds = true;
            if (OrientationManager.Instance.isLandscape)
            {
                AdsManager.Instance.ShowBannerAd();
            }
            else
            {
                // Debug.Log("Portrait");
                AdsManager.Instance.HideBannerAd();
            }
            toggleAds.text = "Disable Ads";
        }
    }


    public void ResetProgress()
    {
        SaveManager.Instance.Reset();

        intervalSlider.value = 60;
        intervalTime.text = "60 mins";

        //currencyInputField.text = "";

        invincibility.text = "Enable Invincibility";
    }
}
