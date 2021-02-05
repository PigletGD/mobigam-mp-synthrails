using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InternetManager : MonoBehaviour
{
    public static InternetManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public bool CheckConnectivity()
    {
        if (!(Application.internetReachability == NetworkReachability.NotReachable))
            return true;
        else return false;
    }
}
