using System;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationHandler : MonoBehaviour
{
    public static NotificationHandler Instance { get; private set; }

    // public string dataText = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            BuildDefaultNotifChannel();
            BuildRepeatNotifChannel();
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        // ProcessData();

        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        SendRepeatNotif();
    }

    private void OnApplicationQuit()
    {
        AndroidNotificationCenter.CancelAllNotifications();
    }

    public void ResetRepeatScheduleNotif()
    {
        CancelScheduledNotifs();

        SendRepeatNotif();
    }

    public void CancelScheduledNotifs()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();
    }

    public void SendSimpleNotif(string title, string text, float seconds)
    {
        DateTime fireTime = DateTime.Now.AddSeconds(seconds);

        var notif = new AndroidNotification(title, text, fireTime);
        AndroidNotificationCenter.SendNotification(notif, "default");
    }

    public void SendRepeatNotif()
    {
        string title = "Repeat Notif";
        int intervalTime = SaveManager.Instance.state.notificationInterval;
        string text = "This repeat notification was sent after " + intervalTime.ToString() + " minutes.";
        DateTime fireTime = DateTime.Now.AddMinutes(intervalTime);
        TimeSpan interval = new TimeSpan(0, intervalTime, 0);

        var notif = new AndroidNotification(title, text, fireTime, interval);
        AndroidNotificationCenter.SendNotification(notif, "repeat");
    }

    public void BuildDefaultNotifChannel()
    {
        string channel_id = "default";
        string channel_name = "Default Channel";
        Importance importance = Importance.Default;
        string channel_description = "App default channel";

        var channel = new AndroidNotificationChannel(channel_id,
                    channel_name, channel_description, importance);

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void BuildRepeatNotifChannel()
    {
        string channel_id = "repeat";
        string channel_name = "Repeating Channel";
        Importance importance = Importance.Default;
        string channel_description = "App repeating notif channel";

        var channel = new AndroidNotificationChannel(channel_id,
                    channel_name, channel_description, importance);

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
}