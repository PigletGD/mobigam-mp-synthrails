using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel = null;
    [SerializeField] private GameObject menuPanel = null;

    private void Awake()
    {
        AudioManager.Instance.PlayLoop("MainMenuMusic");
    }

    public void PlayGame()
    {
        menuPanel.SetActive(true);
        titlePanel.SetActive(false);
    }

    public void QuitGame() => Application.Quit();
}
