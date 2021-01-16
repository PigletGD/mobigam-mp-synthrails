using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject gameHUD = null;

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gameHUD.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gameHUD.SetActive(true);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            Pause();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Pause();
        }
    }
}
