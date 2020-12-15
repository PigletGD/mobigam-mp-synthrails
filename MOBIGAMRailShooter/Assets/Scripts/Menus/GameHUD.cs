using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject portraitUI = null;
    [SerializeField] private GameObject landscapeUI = null;

    [SerializeField] private List<GameObject> heartsL = null;
    [SerializeField] private List<GameObject> heartsP = null;

    private bool isLandscape = false;

    [SerializeField] private PlayerInfo playerInfo = null;

    [SerializeField] Image redAmmoL = null;
    [SerializeField] Image greenAmmoL = null;
    [SerializeField] Image blueAmmoL = null;
    [SerializeField] Image redAmmoP = null;
    [SerializeField] Image greenAmmoP = null;
    [SerializeField] Image blueAmmoP = null;

    [SerializeField] Text redTextL = null;
    [SerializeField] Text greenTextL = null;
    [SerializeField] Text blueTextL = null;
    [SerializeField] Text redTextP = null;
    [SerializeField] Text greenTextP = null;
    [SerializeField] Text blueTextP = null;

    [SerializeField] GameObject resultsPanel = null;
    [SerializeField] Text verdictText = null;
    [SerializeField] Text resourceText = null;

    [SerializeField] ScoreManager SM = null;

    private bool bossDefeated = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            portraitUI.SetActive(true);
            landscapeUI.SetActive(false);

            isLandscape = false;
        }
        else
        {
            portraitUI.SetActive(false);
            landscapeUI.SetActive(true);

            isLandscape = true;
        }

        int heartsToRemove = heartsL.Count - SaveManager.Instance.state.maxHealth;

        for (int i = 0; i < heartsToRemove; i++)
            RemoveHeart();

        UpdateAmmo();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) && isLandscape)
        {
            portraitUI.SetActive(true);
            landscapeUI.SetActive(false);

            isLandscape = false;
        }
        else if ((Screen.orientation == ScreenOrientation.Landscape || Screen.orientation == ScreenOrientation.LandscapeRight) && !isLandscape)
        {
            portraitUI.SetActive(false);
            landscapeUI.SetActive(true);

            isLandscape = true;
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

        if (bossDefeated) verdictText.text = "LEVEL COMPLETE";
        else verdictText.text = "GAME OVER";

        resourceText.text = "SCORE: " + SM.score.ToString() + "\nMONEY COLLECTED: " + SM.moneyCollected.ToString();

        SaveManager.Instance.state.currency += SM.moneyCollected;

        SaveManager.Instance.Save();

        StartCoroutine("ShowResultScreen");
    }

    IEnumerator ShowResultScreen()
    {
        yield return new WaitForSeconds(0.5f);

        portraitUI.SetActive(false);
        landscapeUI.SetActive(false);
        resultsPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        AudioManager.Instance.Stop("GameMusic");
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance.Stop("GameMusic");
        SceneManager.LoadScene("MainMenuScene");
    }

    public void LevelComplete()
    {
        bossDefeated = true;
        SaveManager.Instance.state.unlockedLevelTwo = true;
        DisplayResults();
    }
}