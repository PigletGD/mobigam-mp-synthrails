using UnityEngine;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour
{
    [SerializeField] private Text intervalTime = null;
    [SerializeField] private Slider intervalSlider = null;

    [SerializeField] private InputField currencyInputField = null;

    [SerializeField] private Text invincibility = null;

    public void OnEnable()
    {
        if (SaveManager.Instance.state.invincibilityOn)
            invincibility.text = "Disable Invincibility";
        else invincibility.text = "Enable Invincibility";

        int oldValue = SaveManager.Instance.state.notificationInterval;
        intervalSlider.value = oldValue;
        intervalTime.text = oldValue.ToString() + " mins";
    }

    public void GenerateNotification() => Debug.Log("Generates Notification once Notification System is Implemented");

    public void SetNotificationIntervalTime()
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

    public void ResetProgress()
    {
        SaveManager.Instance.Reset();

        intervalSlider.value = 60;
        intervalTime.text = "60 mins";

        //currencyInputField.text = "";

        invincibility.text = "Enable Invincibility";
    }
}
