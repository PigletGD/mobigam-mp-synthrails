using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public GameObject portraitUI = null;
    public GameObject landscapeUI = null;
    public List<GameObject> heartsL = null;
    public List<GameObject> heartsP = null;
    public PlayerInfo playerInfo = null;
    public Image redAmmoL = null;
    public Image greenAmmoL = null;
    public Image blueAmmoL = null;
    public Image redAmmoP = null;
    public Image greenAmmoP = null;
    public Image blueAmmoP = null;
    public Text redTextL = null;
    public Text greenTextL = null;
    public Text blueTextL = null;
    public Text redTextP = null;
    public Text greenTextP = null;
    public Text blueTextP = null;
    public GameObject resultsPanel = null;
    public Text verdictText = null;
    public Text resourceText = null;
    public ScoreManager SM = null;
    public GameObject playAdPanel = null;
    public Button playAdButton = null;
    public GameObject uploadPanel = null;
    public Text uploadPanelText = null;
    public Button okayButton = null;
    public Button fbButton = null;
    private bool bossDefeated = false;

    // Start is called before the first frame update
    void Start()
    {
        ChangeHUD();

        int heartsToRemove = heartsL.Count - SaveManager.Instance.state.maxHealth;

        for (int i = 0; i < heartsToRemove; i++)
            RemoveHeart();

        UpdateAmmo();

        AdsManager.Instance.OnAdDone += AdManager_OnAdDone;
    }

    private void OnDisable()
    {
        AdsManager.Instance.OnAdDone -= AdManager_OnAdDone;
    }

    public void ChangeHUD()
    {
        if (!OrientationManager.Instance.isLandscape)
        {
            portraitUI.SetActive(true);
            landscapeUI.SetActive(false);
        }
        else
        {
            portraitUI.SetActive(false);
            landscapeUI.SetActive(true);
        }
    }

    public void RemoveHeart()
    {
        heartsL[0].SetActive(false);
        heartsL.RemoveAt(0);

        heartsP[0].SetActive(false);
        heartsP.RemoveAt(0);
    }

    public void UpdateAmmo()
    {
        float ammoCapacity = SaveManager.Instance.state.ammoCapacity;

        redAmmoL.fillAmount = playerInfo.ammoRed / ammoCapacity;
        greenAmmoL.fillAmount = playerInfo.ammoGreen / ammoCapacity;
        blueAmmoL.fillAmount = playerInfo.ammoBlue / ammoCapacity;

        redTextL.text = playerInfo.ammoRed.ToString();
        greenTextL.text = playerInfo.ammoGreen.ToString();
        blueTextL.text = playerInfo.ammoBlue.ToString();

        redAmmoP.fillAmount = playerInfo.ammoRed / ammoCapacity;
        greenAmmoP.fillAmount = playerInfo.ammoGreen / ammoCapacity;
        blueAmmoP.fillAmount = playerInfo.ammoBlue / ammoCapacity;

        redTextP.text = playerInfo.ammoRed.ToString();
        greenTextP.text = playerInfo.ammoGreen.ToString();
        blueTextP.text = playerInfo.ammoBlue.ToString();
    }

    public void AmmoReload(int value)
    {
        switch (value)
        {
            case 0: StartCoroutine(RefillAmmoUI(redAmmoL)); break;
            case 1: StartCoroutine(RefillAmmoUI(greenAmmoL)); break;
            case 2: StartCoroutine(RefillAmmoUI(blueAmmoL)); break;
        }
    }

    IEnumerator RefillAmmoUI(Image image)
    {
        float tick = 0.0f;

        while(tick <= 5.15f)
        {
            tick += Time.deltaTime;

            if (tick > 5.15f)
                tick = 5.15f;

            image.fillAmount = tick / 5.15f;

            yield return null;
        }

        UpdateAmmo();

        yield return null;
    }

    public void DisplayResults()
    {
        RectTransform RT = resultsPanel.GetComponent<RectTransform>();
        RT.sizeDelta = new Vector2(RT.sizeDelta.x, RT.sizeDelta.y) * 0.7f;

        if (bossDefeated) verdictText.text = "LEVEL " + SaveManager.Instance.currentLevel.ToString() + " COMPLETE";
        else verdictText.text = "LEVEL " + SaveManager.Instance.currentLevel.ToString() + " GAME OVER";

        resourceText.text = "SCORE: " + SM.score.ToString() + "\nMONEY COLLECTED: " + SM.moneyCollected.ToString();

        SaveManager.Instance.state.currency += SM.moneyCollected;

        SaveManager.Instance.Save();

        StartCoroutine("ShowResultScreen");
    }

    IEnumerator ShowResultScreen()
    {
        yield return new WaitForSeconds(0.2f);

        portraitUI.SetActive(false);
        landscapeUI.SetActive(false);
        resultsPanel.SetActive(true);

        Debug.Log("Pause");

        Time.timeScale = 0;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        AudioManager.Instance.Stop("GameMusic");
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        AudioManager.Instance.Stop("GameMusic");
        SceneManager.LoadScene("MainMenuScene");
    }

    public void LevelComplete()
    {
        bossDefeated = true;

        switch (SaveManager.Instance.currentLevel)
        {
            case 1: SaveManager.Instance.state.unlockedLevelTwo = true; break;
            case 2: SaveManager.Instance.state.unlockedLevelThree = true; break;
        }

        DisplayResults();
    }

    public void SetActivePlayAd(bool value) => playAdPanel.SetActive(value);

    public void SetActiveUploadPanel(bool value) => uploadPanel.SetActive(value);

    private void AdManager_OnAdDone(object sender, AdFinishEventArgs e)
    {
        if (e.PlacementID == AdsManager.SampleRewardedAd)
        {
            switch (e.AdShowResult)
            {
                case ShowResult.Failed:
                    Debug.Log("Ad failed");
                    Text failText = playAdPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>();
                    failText.text = "Error: Ad failed. Try again?";
                    break;
                case ShowResult.Skipped:
                    Debug.Log("Ad is skipped");
                    Text skipText = playAdPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>();
                    skipText.text = "Error: Ad is skipped. Try again?";
                    break;
                case ShowResult.Finished: Debug.Log("Ad is finished properly");
                    SaveManager.Instance.state.currency += 20;
                    SaveManager.Instance.Save();
                    int money = SM.moneyCollected + 20;
                    resourceText.text = "SCORE: " + SM.score.ToString() + "\nMONEY COLLECTED: " + money.ToString();
                    playAdButton.interactable = false;
                    playAdPanel.SetActive(false);
                    break;
            }
        }
    }
}