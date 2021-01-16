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

    private void OnApplicationPause(bool pause)
    {
        AndroidNotificationCenter.CancelAllNotifications();
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

    public void SendSimpleNotif()
    {
        string title = "Simple Notif";
        string text = "This is a simple notif";
        DateTime fireTime = DateTime.Now.AddSeconds(0.5f);

        var notif = new AndroidNotification(title, text, fireTime);
        AndroidNotificationCenter.SendNotification(notif, "default");
    }

    public void SendRepeatNotif()
    {
        string title = "Repeat Notif";
        string text = "This is a repeating notif";
        int intervalTime = SaveManager.Instance.state.notificationInterval;
        DateTime fireTime = DateTime.Now.AddMinutes(intervalTime);
        TimeSpan interval = new TimeSpan(0, intervalTime, 0);

        var notif = new AndroidNotification(title, text, fireTime, interval);
        AndroidNotificationCenter.SendNotification(notif, "repeat");
    }

    public void SendDataNotif()
    {
        string title = "Data Notif";
        string text = "This is a data notif";
        int intervalTime = SaveManager.Instance.state.notificationInterval;
        DateTime fireTime = DateTime.Now.AddMinutes(intervalTime);
        TimeSpan interval = new TimeSpan(0, intervalTime, 0);

        var notif = new AndroidNotification(title, text, fireTime);

        notif.IntentData = "I HAVE THE POWER OF GOD AND ANIME BY MY SIDE";

        AndroidNotificationCenter.SendNotification(notif, "default");
    }

    public void ProcessData()
    {
        /*
        var data = AndroidNotificationCenter.GetLastNotificationIntent();

        if (data == null)
        {
            dataTxt.gameObject.SetActive(false);
        }
        else
        {
            string dataString = data.Notification.IntentData;
            dataTxt.text = dataString;
        }
        */
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