﻿using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveState state;

    public int currentLevel = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            Load();
        }
        else Destroy(gameObject);
    }

    private void OnApplicationPause() => Save();

    private void OnApplicationQuit() => Save();

    // Save whole state of this saveState script to player pref
    public void Save()
    {
        PlayerPrefs.SetString("Save", SerializationHelper.Serialize<SaveState>(state));
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            state = SerializationHelper.Deserialize<SaveState>(PlayerPrefs.GetString("Save"));

            if (state == null)
            {
                state = new SaveState();

                Save();
            }
        }
        else
        {
            Debug.Log("Creating New Save");
        }
    }

    public void Reset()
    {
        state = new SaveState();

        Save();
    }
}