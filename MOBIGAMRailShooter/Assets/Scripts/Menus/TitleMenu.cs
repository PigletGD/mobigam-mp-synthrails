using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    public GameObject titlePanel = null;
    public GameObject menuPanel = null;

    private void Awake()
    {
        titlePanel = gameObject;
        AudioManager.Instance.PlayLoop("MainMenuMusic");
    }

    public void PlayGame()
    {
        menuPanel.SetActive(true);
        titlePanel.SetActive(false);
    }

    public void QuitGame() => Application.Quit();
}
