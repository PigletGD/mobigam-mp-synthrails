﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour
{
    public GameObject levelTwo = null;
    public GameObject levelThree = null;

    private void OnEnable()
    {
        levelTwo.SetActive(SaveManager.Instance.state.unlockedLevelTwo);
        levelThree.SetActive(SaveManager.Instance.state.unlockedLevelThree);
    }

    public void PlayLevel(int level)
    {
        SaveManager.Instance.currentLevel = level;
        AudioManager.Instance.Stop("MainMenuMusic");
        SceneManager.LoadScene("GameScene");
    }
}
