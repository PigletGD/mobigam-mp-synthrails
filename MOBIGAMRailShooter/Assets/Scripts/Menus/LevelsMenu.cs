using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelTwo = null;
    [SerializeField] private GameObject levelThree = null;

    private void OnEnable()
    {
        levelTwo.SetActive(SaveManager.Instance.state.unlockedLevelTwo);
        levelThree.SetActive(SaveManager.Instance.state.unlockedLevelThree);
    }

    public void PlayLevel()
    {
        AudioManager.Instance.Stop("MainMenuMusic");
        SceneManager.LoadScene("GameScene");
    }
}
