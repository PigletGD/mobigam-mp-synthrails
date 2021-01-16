using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    public static OrientationManager Instance { get; private set; }

    [SerializeField] private GameEventsSO onOrientationChange = null;
    public bool isLandscape = false;

    public float portraitHeight = 0;
    public float uiSizeFactor = 0;
    public float uiPortraitSizeFactor = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (Screen.orientation == ScreenOrientation.Portrait ||
                Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                isLandscape = false;

                portraitHeight = (Screen.height * Screen.height) / Screen.width;
                portraitHeight = portraitHeight / Screen.width;

                uiPortraitSizeFactor = (float)Screen.width / (float)Screen.height;
                uiSizeFactor = uiPortraitSizeFactor;
            }
            else
            {
                isLandscape = true;

                portraitHeight = (Screen.width * Screen.width) / Screen.height;
                portraitHeight = portraitHeight / Screen.height;

                uiPortraitSizeFactor = (float)Screen.height / (float)Screen.width;
                uiSizeFactor = 1;
            }

            onOrientationChange.Raise();
        }
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) && isLandscape)
        {
            isLandscape = false;
            uiSizeFactor = uiPortraitSizeFactor;
            onOrientationChange.Raise();
        }
        else if ((Screen.orientation == ScreenOrientation.Landscape || Screen.orientation == ScreenOrientation.LandscapeRight) && !isLandscape)
        {
            isLandscape = true;
            uiSizeFactor = 1;
            onOrientationChange.Raise();
        }
    }
}
