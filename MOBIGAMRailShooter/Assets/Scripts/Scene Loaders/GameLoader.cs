using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Facebook.Unity;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private string animationBundleName = "";
    [SerializeField] private string audioBundleName = "";
    [SerializeField] private string eventSOBundleName = "";
    [SerializeField] private string singletonBundleName = "";
    [SerializeField] private string volumeProfilesBundleName = "";
    [SerializeField] private string modelsBundleName = "";
    [SerializeField] private string materialsBundleName = "";
    [SerializeField] private string gameSpritesBundleName = "";
    [SerializeField] private string gameEntitiesBundleName = "";
    [SerializeField] private string gameObjectsBundleName = "";

    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.Instance == null)
            LoadSingletons();

        LoadGameObjects();
    }

    private void LoadSingletons()
    {
        GameObject GO = null;

        // Load Save Manager
        GO = BundleManager.Instance.GetAsset<GameObject>(singletonBundleName, "P_SaveManager");
        GO.AddComponent<SaveManager>();
        Instantiate(GO);

        // Load Audio Manager
        GO = BundleManager.Instance.GetAsset<GameObject>(singletonBundleName, "P_AudioManager");
        AudioManager AM = GO.AddComponent<AudioManager>();
        AudioClip audioClip = null;

        // Setup Audioclips
        AM.sounds = new Sound[4];

        audioClip = BundleManager.Instance.GetAsset<AudioClip>(audioBundleName, "A_MainMenuMusic");
        AM.sounds[0] = CreateSound("MainMenuMusic", audioClip, 0.5f, 1.0f);
        audioClip = BundleManager.Instance.GetAsset<AudioClip>(audioBundleName, "A_GameMusic");
        AM.sounds[1] = CreateSound("GameMusic", audioClip, 0.5f, 1.0f);
        audioClip = BundleManager.Instance.GetAsset<AudioClip>(audioBundleName, "A_BulletShot");
        AM.sounds[2] = CreateSound("Shot", audioClip, 0.9f, 1.0f);
        audioClip = BundleManager.Instance.GetAsset<AudioClip>(audioBundleName, "A_Firewall");
        AM.sounds[3] = CreateSound("Firewall", audioClip, 0.9f, 1.0f);

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

    void LoadGameObjects()
    {
        GameObject GO = null;
        GameObject tempObject = null;
        GameObject tempObjectsHolder = null;
        GameEventListener GEL = null;
        string shader = "";

        // Load volume profile bundle
        BundleManager.Instance.LoadBundle(animationBundleName);
        BundleManager.Instance.LoadBundle(gameSpritesBundleName);
        BundleManager.Instance.LoadBundle(gameObjectsBundleName);
        BundleManager.Instance.LoadBundle(gameEntitiesBundleName);
        BundleManager.Instance.LoadBundle(modelsBundleName);
        BundleManager.Instance.LoadBundle(materialsBundleName);

        GO = BundleManager.Instance.GetAsset<GameObject>(gameObjectsBundleName, "P_PostProcessVolume");
        GO = Instantiate(GO);
        Volume volume = GO.AddComponent<Volume>();
        volume.profile = BundleManager.Instance.GetAsset<VolumeProfile>(volumeProfilesBundleName, "VP_SampleSceneProfile");

        // Load Event System
        GO = BundleManager.Instance.GetAsset<GameObject>(gameObjectsBundleName, "P_EventSystem");
        Instantiate(GO);

        // Load Environment
        GO = BundleManager.Instance.GetAsset<GameObject>(gameObjectsBundleName, "P_Environment");
        GO = Instantiate(GO);
        MeshRenderer MeshR = GO.transform.GetChild(0).GetComponent<MeshRenderer>();
        MeshR.material = BundleManager.Instance.GetAsset<Material>(materialsBundleName, "Mat_Wireframe");
        shader = MeshR.material.shader.name;
        MeshR.material.shader = Shader.Find(shader);
        //MeshR.material.EnableKeyword("_EMISSION");
        MeshR = GO.transform.GetChild(1).GetComponent<MeshRenderer>();
        MeshR.material = BundleManager.Instance.GetAsset<Material>(materialsBundleName, "Mat_SynthCircle");
        shader = MeshR.material.shader.name;
        MeshR.material.shader = Shader.Find(shader);
        //MeshR.material.EnableKeyword("_EMISSION");

        // Load
        GameObject canvas = BundleManager.Instance.GetAsset<GameObject>(gameObjectsBundleName, "P_Canvas");
        GameObject player = BundleManager.Instance.GetAsset<GameObject>(gameObjectsBundleName, "P_Player");
        GameObject pools = BundleManager.Instance.GetAsset<GameObject>(gameObjectsBundleName, "P_Pools");
        GameObject systems = BundleManager.Instance.GetAsset<GameObject>(gameObjectsBundleName, "P_Systems");

        // Instantiate
        canvas = Instantiate(canvas);
        player = Instantiate(player);
        pools = Instantiate(pools);
        systems = Instantiate(systems);

        pools.transform.SetAsFirstSibling();

        // Load Pools
        GameObject bullet = BundleManager.Instance.GetAsset<GameObject>(gameEntitiesBundleName, "P_Bullet");
        if (bullet.GetComponent<BulletBehaviour>() == null)
        {
            BulletBehaviour BB = bullet.AddComponent<BulletBehaviour>();
            BB.childTransform = BB.transform.GetChild(0);
            BB.RB = bullet.GetComponent<Rigidbody>();
            /*MeshR = bullet.transform.GetChild(0).GetComponent<MeshRenderer>();
            MeshR.sharedMaterial = BundleManager.Instance.GetAsset<Material>(materialsBundleName, "Mat_Basic");
            shader = MeshR.sharedMaterial.shader.name;
            MeshR.sharedMaterial.shader = Shader.Find(shader);*/
        }
        GameObject virus = BundleManager.Instance.GetAsset<GameObject>(gameEntitiesBundleName, "P_Virus");
        if (virus.GetComponent<EnemyBehaviour>() == null)
        {
            EnemyBehaviour EB = virus.AddComponent<EnemyBehaviour>();
            EB.aimObject = virus.transform.GetChild(1);
        }
        GameObject money = BundleManager.Instance.GetAsset<GameObject>(gameEntitiesBundleName, "P_Money");
        if (money.GetComponent<MoneyBehavior>() == null)
        {
            MoneyBehavior MB = money.AddComponent<MoneyBehavior>();
            MB.RB = money.GetComponent<Rigidbody>();
            /*MeshR = money.transform.GetChild(0).GetComponent<MeshRenderer>();
            MeshR.sharedMaterial = BundleManager.Instance.GetAsset<Material>(materialsBundleName, "Mat_Basic");
            shader = MeshR.sharedMaterial.shader.name;
            MeshR.sharedMaterial.shader = Shader.Find(shader);*/
        }
        GameObject VW = BundleManager.Instance.GetAsset<GameObject>(gameEntitiesBundleName, "P_FirewallVertical");
        GameObject VWMesh = VW.transform.GetChild(0).gameObject;
        if (VWMesh.GetComponent<Firewall>() == null)
        {
            Firewall VWF = VWMesh.AddComponent<Firewall>();
            VWF.warningRT = VW.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
            VWF.canvas = VW.transform.GetChild(1).GetComponent<RectTransform>();
            VWF.isVerticalWall = true;
            GEL = VWMesh.AddComponent<GameEventListener>();
            GEL.Event = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnOrientationChange");
            GEL.Response = new UnityEngine.Events.UnityEvent();
            GEL.Response.AddListener(VWF.ChangeImageRectTransform);
            GEL.RegisterEvent();
        }
        GameObject HW = BundleManager.Instance.GetAsset<GameObject>(gameEntitiesBundleName, "P_FirewallHorizontal");
        GameObject HWMesh = HW.transform.GetChild(0).gameObject;
        if (HWMesh.GetComponent<Firewall>() == null)
        {
            Firewall HWF = HWMesh.AddComponent<Firewall>();
            HWF.warningRT = HW.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
            HWF.canvas = HW.transform.GetChild(1).GetComponent<RectTransform>();
            HWF.isVerticalWall = false;
            GEL = HWMesh.AddComponent<GameEventListener>();
            GEL.Event = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnOrientationChange");
            GEL.Response = new UnityEngine.Events.UnityEvent();
            GEL.Response.AddListener(HWF.ChangeImageRectTransform);
            GEL.RegisterEvent();
        }
        GameObject boss = BundleManager.Instance.GetAsset<GameObject>(gameEntitiesBundleName, "P_Boss");
        if (boss.GetComponent<BossOneBehaviour>() == null)
        {
            BossOneBehaviour BOB = boss.AddComponent<BossOneBehaviour>();
        }

        // Setup pool scripts
        ObjectPool bulletOP = null;
        bulletOP = pools.transform.GetChild(0).gameObject.AddComponent<ObjectPool>();
        bulletOP.poolableObject = bullet;
        bulletOP.initialObjectCapacity = 40;
        ObjectPool explosionOP = null;
        explosionOP = pools.transform.GetChild(1).gameObject.AddComponent<ObjectPool>();
        explosionOP.poolableObject = BundleManager.Instance.GetAsset<GameObject>(gameEntitiesBundleName, "P_Explosion");
        explosionOP.initialObjectCapacity = 5;
        ObjectPool virusOP = null;
        virusOP = pools.transform.GetChild(2).gameObject.AddComponent<ObjectPool>();
        virusOP.poolableObject = virus;
        virusOP.initialObjectCapacity = 6;
        ObjectPool moneyOP = null;
        moneyOP = pools.transform.GetChild(3).gameObject.AddComponent<ObjectPool>();
        moneyOP.poolableObject = money;
        moneyOP.initialObjectCapacity = 15;
        ObjectPool VWOP = null;
        VWOP = pools.transform.GetChild(4).gameObject.AddComponent<ObjectPool>();
        VWOP.poolableObject = VW;
        VWOP.initialObjectCapacity = 5;
        ObjectPool HWOP = null;
        HWOP = pools.transform.GetChild(5).gameObject.AddComponent<ObjectPool>();
        HWOP.poolableObject = HW;
        HWOP.initialObjectCapacity = 5;

        //Instantiate(pools);

        // Load Canvas
        GameObject gameHUDPanel = canvas.transform.GetChild(0).gameObject;
        GameObject resultsPanel = canvas.transform.GetChild(1).gameObject;
        GameObject pauseMenu = canvas.transform.GetChild(2).gameObject;
        GameObject portraitUI = gameHUDPanel.transform.GetChild(1).gameObject;
        GameObject landscapeUI = gameHUDPanel.transform.GetChild(2).gameObject;

        PauseMenu PM = canvas.AddComponent<PauseMenu>();
        PM.gameHUD = gameHUDPanel;
        PM.pauseMenu = pauseMenu;

        GameHUD gameHUD = canvas.AddComponent<GameHUD>();
        gameHUD.portraitUI = portraitUI;
        gameHUD.landscapeUI = landscapeUI;
        tempObjectsHolder = portraitUI.transform.GetChild(0).GetChild(1).gameObject;
        gameHUD.heartsL = new List<GameObject>();
        gameHUD.heartsL.Add(tempObjectsHolder.transform.GetChild(5).gameObject);
        gameHUD.heartsL.Add(tempObjectsHolder.transform.GetChild(4).gameObject);
        gameHUD.heartsL.Add(tempObjectsHolder.transform.GetChild(3).gameObject);
        gameHUD.heartsL.Add(tempObjectsHolder.transform.GetChild(2).gameObject);
        gameHUD.heartsL.Add(tempObjectsHolder.transform.GetChild(1).gameObject);
        gameHUD.heartsL.Add(tempObjectsHolder.transform.GetChild(0).gameObject);
        tempObjectsHolder = landscapeUI.transform.GetChild(1).gameObject;
        gameHUD.heartsP = new List<GameObject>();
        gameHUD.heartsP.Add(tempObjectsHolder.transform.GetChild(5).gameObject);
        gameHUD.heartsP.Add(tempObjectsHolder.transform.GetChild(4).gameObject);
        gameHUD.heartsP.Add(tempObjectsHolder.transform.GetChild(3).gameObject);
        gameHUD.heartsP.Add(tempObjectsHolder.transform.GetChild(2).gameObject);
        gameHUD.heartsP.Add(tempObjectsHolder.transform.GetChild(1).gameObject);
        gameHUD.heartsP.Add(tempObjectsHolder.transform.GetChild(0).gameObject);
        //DO PLAYER INFO REFERENCE LATER
        tempObjectsHolder = portraitUI.transform.GetChild(0).GetChild(2).gameObject;
        tempObject = tempObjectsHolder.transform.GetChild(1).gameObject;
        gameHUD.redAmmoP = tempObject.transform.GetChild(0).GetComponent<Image>();
        gameHUD.redTextP = tempObject.transform.GetChild(1).GetComponent<Text>();
        tempObject = tempObjectsHolder.transform.GetChild(2).gameObject;
        gameHUD.greenAmmoP = tempObject.transform.GetChild(0).GetComponent<Image>();
        gameHUD.greenTextP = tempObject.transform.GetChild(1).GetComponent<Text>();
        tempObject = tempObjectsHolder.transform.GetChild(3).gameObject;
        gameHUD.blueAmmoP = tempObject.transform.GetChild(0).GetComponent<Image>();
        gameHUD.blueTextP = tempObject.transform.GetChild(1).GetComponent<Text>();
        tempObjectsHolder = landscapeUI.transform.GetChild(2).gameObject;
        tempObject = tempObjectsHolder.transform.GetChild(1).gameObject;
        gameHUD.redAmmoL = tempObject.transform.GetChild(0).GetComponent<Image>();
        gameHUD.redTextL = tempObject.transform.GetChild(1).GetComponent<Text>();
        tempObject = tempObjectsHolder.transform.GetChild(2).gameObject;
        gameHUD.greenAmmoL = tempObject.transform.GetChild(0).GetComponent<Image>();
        gameHUD.greenTextL = tempObject.transform.GetChild(1).GetComponent<Text>();
        tempObject = tempObjectsHolder.transform.GetChild(3).gameObject;
        gameHUD.blueAmmoL = tempObject.transform.GetChild(0).GetComponent<Image>();
        gameHUD.blueTextL = tempObject.transform.GetChild(1).GetComponent<Text>();
        gameHUD.resultsPanel = resultsPanel;
        gameHUD.verdictText = resultsPanel.transform.GetChild(0).GetComponent<Text>();
        gameHUD.resourceText = resultsPanel.transform.GetChild(1).GetComponent<Text>();
        //DO SCORE MANAGER LATER

        GEL = canvas.AddComponent<GameEventListener>();
        GEL.Event = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnBossKilled");
        GEL.Response = new UnityEngine.Events.UnityEvent();
        GEL.Response.AddListener(gameHUD.LevelComplete);
        GEL.RegisterEvent();
        GEL = canvas.AddComponent<GameEventListener>();
        GEL.Event = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnOrientationChange");
        GEL.Response = new UnityEngine.Events.UnityEvent();
        GEL.Response.AddListener(gameHUD.ChangeHUD);
        GEL.RegisterEvent();

        // Tap panels
        GameObject tapPanel = null;
        GameObject joystick = null;
        tapPanel = gameHUDPanel.transform.GetChild(0).GetChild(0).gameObject;
        joystick = tapPanel.transform.GetChild(0).gameObject;
        JoystickHolder JH = tapPanel.AddComponent<JoystickHolder>();
        OnScreenStick MOSS = joystick.AddComponent<OnScreenStick>();
        JH.Joystick = MOSS;
        JH.hitBox = tapPanel.GetComponent<Image>();
        MOSS.JoystickParent = joystick.GetComponent<Image>();
        MOSS.Stick = joystick.transform.GetChild(0).GetComponent<Image>();
        tapPanel = gameHUDPanel.transform.GetChild(0).GetChild(1).gameObject;
        joystick = tapPanel.transform.GetChild(0).gameObject;
        TouchPanel TP = tapPanel.AddComponent<TouchPanel>();
        OnScreenStick AOSS = joystick.AddComponent<OnScreenStick>();
        TP.Joystick = AOSS;
        TP.hitBox = tapPanel.GetComponent<Image>();
        AOSS.JoystickParent = joystick.GetComponent<Image>();
        AOSS.Stick = joystick.transform.GetChild(0).GetComponent<Image>();
        
        // Buttons
        portraitUI.transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(PM.Pause);
        landscapeUI.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(PM.Pause);
        tempObjectsHolder = resultsPanel.transform.GetChild(2).gameObject;
        tempObjectsHolder.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(gameHUD.PlayAgain);
        tempObjectsHolder.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(gameHUD.GoToMainMenu);
        tempObjectsHolder.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { gameHUD.SetActivePlayAd(true); });
        tempObjectsHolder.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { FBManager.Instance.UploadScreenshot(); });
        gameHUD.fbButton = tempObjectsHolder.transform.GetChild(3).GetComponent<Button>();
        gameHUD.playAdButton = tempObjectsHolder.transform.GetChild(2).GetComponent<Button>();
        tempObjectsHolder = resultsPanel.transform.GetChild(3).gameObject;
        gameHUD.playAdPanel = tempObjectsHolder;
        tempObjectsHolder.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => { AdsManager.Instance.ShowRewardedAd(); });
        tempObjectsHolder.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => { gameHUD.SetActivePlayAd(false); });
        tempObjectsHolder = pauseMenu.transform.GetChild(1).gameObject;
        tempObjectsHolder.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(PM.Resume);
        tempObjectsHolder.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(gameHUD.PlayAgain);
        tempObjectsHolder.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(gameHUD.GoToMainMenu);
        gameHUD.uploadPanel = resultsPanel.transform.GetChild(4).gameObject;
        gameHUD.uploadPanelText = gameHUD.uploadPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        gameHUD.okayButton = gameHUD.uploadPanel.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        gameHUD.okayButton.onClick.AddListener(() => { gameHUD.SetActiveUploadPanel(false); if (FBManager.Instance.uploadSuccessful) gameHUD.fbButton.interactable = false; });
        //Instantiate(GO);

        // Load Player
        GameObject playerPlane = player.transform.GetChild(0).gameObject;
        GameObject playerCameraHolder = player.transform.GetChild(2).gameObject;

        PlayerInfo PI = playerPlane.AddComponent<PlayerInfo>();
        PI.touchPanel = TP;
        PI.explosionPool = explosionOP;
        PI.gameHUD = gameHUD;
        gameHUD.playerInfo = PI;
        PI.onPlayerDeath = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnPlayerKilled");

        PlayerMovement PMove = playerPlane.AddComponent<PlayerMovement>();
        PMove.planeModelTransform = playerPlane.transform.GetChild(0);
        PMove.aimTargetTransform = player.transform.GetChild(1).GetChild(0);
        PMove.playerInfo = PI;
        PMove.moveJoystick = MOSS;
        PMove.touchPanel = TP;
        PMove.aimJoystick = AOSS;
        PMove.playerAnim = playerPlane.GetComponent<Animator>();

        PlayerShooting PS = playerPlane.AddComponent<PlayerShooting>();
        PS.touchPanel = TP;
        PS.playerInfo = PI;
        PS.planeNuzzle = playerPlane.transform.GetChild(0).GetChild(0).GetChild(0);
        PS.bulletPool = bulletOP;
        PS.gameHUD = gameHUD;

        GameObject camera = playerCameraHolder.transform.GetChild(0).gameObject;
        camera.tag = "MainCamera";
        Camera camComp = camera.AddComponent<Camera>();
        camera.AddComponent<UniversalAdditionalCameraData>();
        CameraOrienter CO = playerCameraHolder.AddComponent<CameraOrienter>();
        camera.gameObject.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
        camComp.clearFlags = CameraClearFlags.SolidColor;
        camComp.backgroundColor = Color.black;
        CO.cam = camComp;
        GEL = playerCameraHolder.AddComponent<GameEventListener>();
        GEL.Event = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnOrientationChange");
        GEL.Response = new UnityEngine.Events.UnityEvent();
        GEL.Response.AddListener(CO.ChangeFOV);
        GEL.RegisterEvent();

        GameObject crosshair = player.transform.GetChild(3).GetChild(0).gameObject;
        Crosshair CH = crosshair.AddComponent<Crosshair>();
        CH.RT = crosshair.GetComponent<RectTransform>();
        CH.playerModel = PMove.planeModelTransform;
        CH.canvas = player.transform.GetChild(3).GetComponent<Canvas>();
        GEL = crosshair.AddComponent<GameEventListener>();
        GEL.Event = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnOrientationChange");
        GEL.Response = new UnityEngine.Events.UnityEvent();
        GEL.Response.AddListener(gameHUD.ChangeHUD);
        GEL.RegisterEvent();

        // Load Systems
        GameObject emissionControl = systems.transform.GetChild(0).gameObject;
        GameObject scoreManager = systems.transform.GetChild(1).gameObject;
        GameObject waveManager = systems.transform.GetChild(2).gameObject;

        /*EmissionControl EC = emissionControl.AddComponent<EmissionControl>();
        EC.materials = new List<Material>();
        EC.materials.Add(BundleManager.Instance.GetAsset<Material>(materialsBundleName, "Mat_Ship"));
        EC.materials.Add(BundleManager.Instance.GetAsset<Material>(materialsBundleName, "Mat_Wireframe"));*/

        WaveManager WM = waveManager.AddComponent<WaveManager>();
        WM.virusPool = virusOP;
        WM.moneyPool = moneyOP;
        WM.verticalWallPool = VWOP;
        WM.horizontalWallPool = HWOP;
        WM.boss = boss;
        WM.cam = camComp;
        GEL = waveManager.AddComponent<GameEventListener>();
        GEL.Event = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnEnemyKilled");
        GEL.Response = new UnityEngine.Events.UnityEvent();
        GEL.Response.AddListener(WM.DecreaseEnemyCount);
        GEL.RegisterEvent();

        ScoreManager SM = scoreManager.AddComponent<ScoreManager>();
        SM.portraitText = portraitUI.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        SM.landscapeText = landscapeUI.transform.GetChild(0).GetComponent<Text>();
        GEL = scoreManager.AddComponent<GameEventListener>();
        GEL.Event = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnEnemyKilled");
        GEL.Response = new UnityEngine.Events.UnityEvent();
        GEL.Response.AddListener(() => { SM.AddScore(100); });
        GEL.RegisterEvent();
        GEL = scoreManager.AddComponent<GameEventListener>();
        GEL.Event = BundleManager.Instance.GetAsset<GameEventsSO>(eventSOBundleName, "EventSO_OnBossKilled");
        GEL.Response = new UnityEngine.Events.UnityEvent();
        GEL.Response.AddListener(() => { SM.AddScore(5000 * SaveManager.Instance.currentLevel); });
        GEL.RegisterEvent();
        gameHUD.SM = SM;

        //player.transform.GetChild(0).GetComponent<PlayerMovement>().touchPanel = canvas.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TouchPanel>();
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
}
