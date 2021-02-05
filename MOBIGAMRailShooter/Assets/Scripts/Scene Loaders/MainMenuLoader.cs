using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class MainMenuLoader : MonoBehaviour
{
    [SerializeField] private string audioBundleName = "";
    [SerializeField] private string eventSOBundleName = "";
    [SerializeField] private string singletonBundleName = ""; 
    [SerializeField] private string mainMenuSpritesBundleName = "";
    [SerializeField] private string mainMenuObjectsBundleName = "";
    

    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.Instance == null)
            LoadSingletons();

        LoadMainMenu();

        if (AdsManager.Instance != null)
            AdsManager.Instance.ShowBannerAd();

        GameEventListener GEL = gameObject.AddComponent<GameEventListener>();
        GEL.Event = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnOrientationChange");
        GEL.Response = new UnityEngine.Events.UnityEvent();
        GEL.Response.AddListener(HandleBannerSwitch);
        GEL.RegisterEvent();

        HandleBannerSwitch();
    }

    private void LoadSingletons()
    {
        GameObject GO = null;

        // Load Save Manager
        GO = BundleManager.Instance.GetAsset<GameObject>(singletonBundleName, "P_SaveManager");
        GO.AddComponent<SaveManager>();
        Instantiate(GO);

        // Load Internet Manager
        GO = BundleManager.Instance.GetAsset<GameObject>(singletonBundleName, "P_InternetManager");
        GO.AddComponent<InternetManager>();
        Instantiate(GO);

        // Load Audio Manager
        GO = BundleManager.Instance.GetAsset<GameObject>(singletonBundleName, "P_AudioManager");
        AudioManager AM = GO.AddComponent<AudioManager>();
        AudioClip audioClip = null;

        // Setup Audioclips
        AM.sounds = new Sound[5];

        audioClip = BundleManager.Instance.GetAsset<AudioClip>(audioBundleName, "A_MainMenuMusic");
        AM.sounds[0] = CreateSound("MainMenuMusic", audioClip, 0.5f, 1.0f);
        audioClip = BundleManager.Instance.GetAsset<AudioClip>(audioBundleName, "A_GameMusic");
        AM.sounds[1] = CreateSound("GameMusic", audioClip, 0.5f, 1.0f);
        audioClip = BundleManager.Instance.GetAsset<AudioClip>(audioBundleName, "A_BulletShot");
        AM.sounds[2] = CreateSound("Shot", audioClip, 0.9f, 1.0f);
        audioClip = BundleManager.Instance.GetAsset<AudioClip>(audioBundleName, "A_Firewall");
        AM.sounds[3] = CreateSound("Firewall", audioClip, 0.9f, 1.0f);
        audioClip = BundleManager.Instance.GetAsset<AudioClip>(audioBundleName, "A_Explosion");
        AM.sounds[4] = CreateSound("Explosion", audioClip, 0.9f, 1.0f);

        // Initialize sound instances
        AM.InitializeSounds();

        Instantiate(GO);

        // Load Orientation Manager
        GO = BundleManager.Instance.GetAsset<GameObject>(singletonBundleName, "P_OrientationManager");
        OrientationManager OM = GO.AddComponent<OrientationManager>();
        OM.onOrientationChange = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnOrientationChange");
        Instantiate(GO);

        // Load Notification Manager
        GO = BundleManager.Instance.GetAsset<GameObject>(singletonBundleName, "P_NotificationHandler");
        GO.AddComponent<NotificationHandler>();
        Instantiate(GO);

        // Load Ads Manager
        GO = BundleManager.Instance.GetAsset<GameObject>(singletonBundleName, "P_AdsManager");
        GO.AddComponent<AdsManager>();
        Instantiate(GO);

        // Load FB Manager
        GO = BundleManager.Instance.GetAsset<GameObject>(singletonBundleName, "P_FBManager");
        GO.AddComponent<FBManager>();
        Instantiate(GO);
    }

    private void LoadMainMenu()
    {
        GameObject GO = null;

        // Load Main Menu Sprites
        BundleManager.Instance.LoadBundle(mainMenuSpritesBundleName);

        // Load Main Menu Camera
        GO = BundleManager.Instance.GetAsset<GameObject>(mainMenuObjectsBundleName, "P_MainCamera");
        GO = Instantiate(GO);
        GO.AddComponent<Camera>();
        GO.AddComponent<UniversalAdditionalCameraData>();

        // Load Event System
        GO = BundleManager.Instance.GetAsset<GameObject>(mainMenuObjectsBundleName, "P_EventSystem");
        Instantiate(GO);

        // Load Main Menu Canvas
        GO = BundleManager.Instance.GetAsset<GameObject>(mainMenuObjectsBundleName, "P_MainMenuCanvas");
        
        // Instantiate main menu canvas
        GameObject canvasGO = Instantiate(GO);

        // Setup Title Menu
        GameObject titlePanel = canvasGO.transform.GetChild(1).gameObject;
        GameObject menuPanel = canvasGO.transform.GetChild(2).gameObject;
        TitleMenu TM = titlePanel.AddComponent<TitleMenu>();
        TM.menuPanel = menuPanel;

        // Grab menu panels
        GameObject menuViewPanels = menuPanel.transform.GetChild(1).gameObject;
        GameObject levelPanel = menuViewPanels.transform.GetChild(0).gameObject;
        GameObject shopPanel = menuViewPanels.transform.GetChild(1).gameObject;
        GameObject configPanel = menuViewPanels.transform.GetChild(2).gameObject;
        
        // Setup Menu Navigator for text changing in buttons
        GameObject buttonsPanel = menuPanel.transform.GetChild(0).gameObject;
        GameObject buttonOne = buttonsPanel.transform.GetChild(0).gameObject;
        GameObject buttonTwo = buttonsPanel.transform.GetChild(1).gameObject;
        MenuNavigator MN = buttonsPanel.AddComponent<MenuNavigator>();
        MN.levelMenu = levelPanel;
        MN.shopMenu = shopPanel;
        MN.configMenu = configPanel;
        MN.buttonOne = buttonOne.transform.GetChild(0).GetComponent<Text>();
        MN.buttonTwo = buttonTwo.transform.GetChild(0).GetComponent<Text>();

        // Get level menu game objects
        GameObject levelViewPanel = levelPanel.transform.GetChild(1).gameObject;
        GameObject levelOneButtonGO = levelViewPanel.transform.GetChild(0).gameObject;
        GameObject levelTwoButtonGO = levelViewPanel.transform.GetChild(1).gameObject;
        GameObject levelThreeButtonGO = levelViewPanel.transform.GetChild(2).gameObject;

        // Attach level menu script
        LevelsMenu LM = levelPanel.AddComponent<LevelsMenu>();
        LM.levelTwo = levelTwoButtonGO;
        LM.levelThree = levelThreeButtonGO;

        //  Get shop menu game objects
        GameObject shopViewPanel = shopPanel.transform.GetChild(2).gameObject;
        GameObject healthButtonHolder = shopViewPanel.transform.GetChild(0).GetChild(0).gameObject;
        GameObject speedButtonHolder = shopViewPanel.transform.GetChild(0).GetChild(1).gameObject;
        GameObject damageButtonHolder = shopViewPanel.transform.GetChild(1).GetChild(0).gameObject;
        GameObject ammoButtonHolder = shopViewPanel.transform.GetChild(1).GetChild(1).gameObject;

        // Attach and set up shop menu script
        ShopMenu SM = shopPanel.AddComponent<ShopMenu>();
        SM.currency = shopPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        SM.health = healthButtonHolder.transform.GetChild(1).GetComponent<Text>();
        SM.speed = speedButtonHolder.transform.GetChild(1).GetComponent<Text>();
        SM.damage = damageButtonHolder.transform.GetChild(1).GetComponent<Text>();
        SM.ammo = ammoButtonHolder.transform.GetChild(1).GetComponent<Text>();
        SM.healthUpgrades = new List<Upgrade>();
        SM.healthUpgrades.Add(CreateUpgrade(20, "Health I Cost: 20\n(Current Health: 3)", 1));
        SM.healthUpgrades.Add(CreateUpgrade(50, "Health II Cost: 50\n(Current Health: 4)", 1));
        SM.healthUpgrades.Add(CreateUpgrade(100, "Health III Cost: 100\n(Current Health: 5)", 1));
        SM.speedUpgrades = new List<Upgrade>();
        SM.speedUpgrades.Add(CreateUpgrade(20, "Speed I Cost: 20\n(Current Speed: 8)", 3));
        SM.speedUpgrades.Add(CreateUpgrade(50, "Speed II Cost: 50\n(Current Speed: 11)", 3));
        SM.speedUpgrades.Add(CreateUpgrade(100, "Speed III Cost: 100\n(Current Speee: 14)", 3));
        SM.damageUpgrades = new List<Upgrade>();
        SM.damageUpgrades.Add(CreateUpgrade(20, "Damage I Cost: 20\n(Current Damage: 1)", 1));
        SM.damageUpgrades.Add(CreateUpgrade(50, "Damage II Cost: 50\n(Current Damage: 2)", 1));
        SM.damageUpgrades.Add(CreateUpgrade(100, "Damage III Cost: 100\n(Current Damage: 3)", 1));
        SM.ammoUpgrades = new List<Upgrade>();
        SM.ammoUpgrades.Add(CreateUpgrade(20, "Ammo I Cost: 20\n(Current Ammo: 30)", 10));
        SM.ammoUpgrades.Add(CreateUpgrade(50, "Ammo II Cost: 50\n(Current Ammo: 40)", 10));
        SM.ammoUpgrades.Add(CreateUpgrade(100, "Ammo III Cost: 100\n(Current Ammo: 50)", 10));

        // Get config menu game objects
        GameObject configViewPanel = configPanel.transform.GetChild(1).gameObject;
        GameObject simpleNotifButtonHolder = configViewPanel.transform.GetChild(0).gameObject;
        GameObject repeatNotifSliderHolder = configViewPanel.transform.GetChild(1).GetChild(1).gameObject;
        GameObject unlockLevelsButtonHolder = configViewPanel.transform.GetChild(2).gameObject;
        GameObject addCurrencyPanel = configViewPanel.transform.GetChild(3).gameObject;
        GameObject invincibilityButtonHolder = configViewPanel.transform.GetChild(4).gameObject;
        GameObject resetButtonHolder = configViewPanel.transform.GetChild(5).gameObject;
        GameObject playAdButtonHolder = configViewPanel.transform.GetChild(6).gameObject;
        GameObject toggleAdButtonHolder = configViewPanel.transform.GetChild(7).gameObject;

        // Attach and setup config menu script
        ConfigMenu CM = configPanel.AddComponent<ConfigMenu>();
        CM.intervalTime = configViewPanel.transform.GetChild(1).GetChild(1).GetChild(3).GetComponent<Text>();
        CM.intervalSlider = repeatNotifSliderHolder.GetComponent<Slider>();
        CM.currencyInputField = addCurrencyPanel.transform.GetChild(1).GetComponent<InputField>();
        CM.invincibility = invincibilityButtonHolder.transform.GetChild(0).GetComponent<Text>();
        CM.playAdText = playAdButtonHolder.transform.GetChild(0).GetComponent<Text>();
        CM.toggleAds = toggleAdButtonHolder.transform.GetChild(0).GetComponent<Text>();

        // Setup events on title buttons
        Button startButton = titlePanel.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        startButton.onClick.AddListener(TM.PlayGame);
        Button quitButton = titlePanel.transform.GetChild(1).GetChild(1).GetComponent<Button>();
        quitButton.onClick.AddListener(TM.QuitGame);

        // Setup events on menu navigator buttons
        Button leftButton = buttonOne.GetComponent<Button>();
        leftButton.onClick.AddListener(MN.LeftMenuButton);
        Button rightButton = buttonTwo.GetComponent<Button>();
        rightButton.onClick.AddListener(MN.RightMenuButton);

        // Setup events on level menu buttons
        Button levelOneButton = levelOneButtonGO.GetComponent<Button>();
        levelOneButton.onClick.AddListener(() => { LM.PlayLevel(1); });
        Button levelTwoButton = levelTwoButtonGO.GetComponent<Button>();
        levelTwoButton.onClick.AddListener(() => { LM.PlayLevel(2); });
        Button levelThreeButton = levelThreeButtonGO.GetComponent<Button>();
        levelThreeButton.onClick.AddListener(() => { LM.PlayLevel(3); });

        // Setup events on shop menu buttons
        Button healthButton = healthButtonHolder.transform.GetChild(0).GetComponent<Button>();
        healthButton.onClick.AddListener(SM.BuyHealth);
        Button speedButton = speedButtonHolder.transform.GetChild(0).GetComponent<Button>();
        speedButton.onClick.AddListener(SM.BuySpeed);
        Button damageButton = damageButtonHolder.transform.GetChild(0).GetComponent<Button>();
        damageButton.onClick.AddListener(SM.BuyDamage);
        Button ammoButton = ammoButtonHolder.transform.GetChild(0).GetComponent<Button>();
        ammoButton.onClick.AddListener(SM.BuyAmmo);

        // Setup events in config menu objects
        Button simpleNotifButton = simpleNotifButtonHolder.GetComponent<Button>();
        simpleNotifButton.onClick.AddListener(CM.GenerateNotification);
        CM.intervalSlider.onValueChanged.AddListener(CM.SetNotificationIntervalTime);
        Button unlockLevelsButton = unlockLevelsButtonHolder.GetComponent<Button>();
        unlockLevelsButton.onClick.AddListener(CM.UnlockAllLevels);
        Button addCurrencyButton = addCurrencyPanel.transform.GetChild(0).GetComponent<Button>();
        addCurrencyButton.onClick.AddListener(CM.AddCurrency);
        Button invincibilityButton = invincibilityButtonHolder.GetComponent<Button>();
        invincibilityButton.onClick.AddListener(CM.Invincibility);
        Button resetButton = resetButtonHolder.GetComponent<Button>();
        resetButton.onClick.AddListener(CM.ResetProgress);
        Button playAdButton = playAdButtonHolder.GetComponent<Button>();
        playAdButton.onClick.AddListener(CM.PlayVideoAd);
        Button toggleAdButton = toggleAdButtonHolder.GetComponent<Button>();
        toggleAdButton.onClick.AddListener(CM.ToggleAds);
    }

    private Sound CreateSound(string name, AudioClip audioClip, float volume, float pitch)
    {
        Sound newSound = new Sound();

        newSound.name = name;
        newSound.clip = audioClip;
        newSound.volume = volume;
        newSound.pitch = pitch;

        return newSound;
    }

    private Upgrade CreateUpgrade(int cost, string description, int value)
    {
        Upgrade newUpgrade = new Upgrade();

        newUpgrade.upgradeCost = cost;
        newUpgrade.upgradeDescription = description;
        newUpgrade.upgradeValue = value;

        return newUpgrade;
    }

    private void HandleBannerSwitch()
    {
        StopAllCoroutines();

        if (AdsManager.Instance.showAds)
            StartCoroutine("HandleBannerSwitch_Routine");
    }

    IEnumerator HandleBannerSwitch_Routine()
    {
        yield return new WaitForSeconds(0.1f);

        if (OrientationManager.Instance.isLandscape)
        {
            AdsManager.Instance.ShowBannerAd();
        }
        else 
        {
            // Debug.Log("Portrait");
            AdsManager.Instance.HideBannerAd();
        }
    }
}