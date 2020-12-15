using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject portraitUI;
    [SerializeField] private GameObject landscapeUI;

    [SerializeField] private List<GameObject> heartsL;
    [SerializeField] private List<GameObject> heartsP;

    private bool isLandscape = false;

    float portraitHeightGV;
    float portraitHeightF;

    [SerializeField] private PlayerInfo playerInfo;

    [SerializeField] Image redAmmoL;
    [SerializeField] Image greenAmmoL;
    [SerializeField] Image blueAmmoL;
    [SerializeField] Image redAmmoP;
    [SerializeField] Image greenAmmoP;
    [SerializeField] Image blueAmmoP;

    [SerializeField] Text redTextL;
    [SerializeField] Text greenTextL;
    [SerializeField] Text blueTextL;
    [SerializeField] Text redTextP;
    [SerializeField] Text greenTextP;
    [SerializeField] Text blueTextP;

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
}