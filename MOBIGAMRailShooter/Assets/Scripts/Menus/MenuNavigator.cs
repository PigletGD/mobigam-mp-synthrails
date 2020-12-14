using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    [SerializeField] private GameObject levelMenu = null;
    [SerializeField] private GameObject shopMenu = null;
    [SerializeField] private GameObject configMenu = null;

    private string currentText = null;
    [SerializeField] private Text buttonOne = null;
    [SerializeField] private Text buttonTwo = null;

    private GameObject currentMenu = null;
    private GameObject menuButtonOne = null;
    private GameObject menuButtonTwo = null;

    private void Start()
    {
        currentText = "Level";
        buttonOne.text = "Shop";
        buttonTwo.text = "Config";

        currentMenu = levelMenu;
        menuButtonOne = shopMenu;
        menuButtonTwo = configMenu;

        currentMenu.SetActive(true);
        menuButtonOne.SetActive(false);
        menuButtonTwo.SetActive(false);
    }

    public void LeftMenuButton()
    {
        SwitchMenus(menuButtonOne);

        string tempText = currentText;
        currentText = buttonOne.text;
        buttonOne.text = tempText;

        GameObject tempMenu = currentMenu;
        currentMenu = menuButtonOne;
        menuButtonOne = tempMenu;
    }

    public void RightMenuButton()
    {
        SwitchMenus(menuButtonTwo);

        string tempText = currentText;
        currentText = buttonTwo.text;
        buttonTwo.text = tempText;

        GameObject tempMenu = currentMenu;
        currentMenu = menuButtonTwo;
        menuButtonTwo = tempMenu;
    }

    private void SwitchMenus(GameObject setMenu)
    {
        currentMenu.SetActive(false);
        setMenu.SetActive(true);
    }
}