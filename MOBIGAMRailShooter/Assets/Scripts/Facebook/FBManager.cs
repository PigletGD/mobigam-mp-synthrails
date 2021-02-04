using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FBManager : MonoBehaviour
{
    public static FBManager Instance { get; private set; }

    private bool aboutToUpload = false;
    public bool uploadSuccessful = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!FB.IsInitialized)
                FB.Init(OnFBInitialize, OnHideFB);
            else FB.ActivateApp();

            Debug.Log("Facebook Manager Instantiated");
        }
        else Destroy(gameObject);
    }

    private void OnFBInitialize()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("FB Done Init");
        }
        else Debug.LogError("Failed to Init FB");
    }

    private void OnHideFB(bool shown)
    {
        if (shown) Time.timeScale = 1;
        else Time.timeScale = 0;
    }

    private void FBLoginDone(ILoginResult res)
    {
        if (FB.IsLoggedIn)
        {
            if (aboutToUpload) UploadScreenshot();
            else Debug.Log("Login successful");
        }
        else
        {
            GameHUD gameHUD = FindObjectOfType<GameHUD>();
            if (gameHUD != null)
            {
                gameHUD.okayButton.gameObject.SetActive(true);
                gameHUD.uploadPanelText.text = "Error: User failed to login";
                Debug.Log("Changed text to login fail");
            }
        }
    }

    public void LoginFB()
    {
        if (FB.IsInitialized)
        {
            if (!FB.IsLoggedIn)
            {
                List<string> permissions = new List<string>() { "public_profile", "email" };
                FB.LogInWithReadPermissions(permissions, FBLoginDone);
            }
            else if (aboutToUpload)
            {
                UploadScreenshot();
            }
        }
        else
        {
            GameHUD gameHUD = FindObjectOfType<GameHUD>();
            if (gameHUD != null)
            {
                gameHUD.okayButton.gameObject.SetActive(true);
                gameHUD.uploadPanelText.text = "Error: Facebook initialization fail";
                Debug.Log("Changed text to fb failed initialization");
            }

            Debug.LogError("FB not yet initialized");
        }
    }

    private void UploadPhotoDone (IGraphResult res)
    {
        GameHUD gameHUD = FindObjectOfType<GameHUD>();

        if (string.IsNullOrEmpty(res.Error))
        {
            if (gameHUD != null)
            {
                gameHUD.okayButton.gameObject.SetActive(true);
                gameHUD.uploadPanelText.text = "Upload done!";
            }

            uploadSuccessful = true;

            Debug.Log("Uploaded photo with id: " + res.ResultDictionary["id"].ToString());
        }
        else
        {
            if (gameHUD != null)
            {
                gameHUD.okayButton.gameObject.SetActive(true);
                gameHUD.uploadPanelText.text = "Error uploading photo" + res.Error;
            }

            Debug.Log("Error uploading photo" + res.Error);
        }
    }

    IEnumerator ScreenshotAndUpload()
    {
        GameHUD gameHUD = FindObjectOfType<GameHUD>();
        if (gameHUD != null)
            gameHUD.uploadPanel.SetActive(false);
        AdsManager.Instance.HideBannerAd();

        yield return new WaitForEndOfFrame();
        Texture2D screen = ScreenCapture.CaptureScreenshotAsTexture();
        byte[] screenBytes = screen.EncodeToPNG();

        WWWForm form = new WWWForm();
        //Add the bytes and name the image to be sent
        form.AddBinaryData("image", screenBytes, "screenshot.png");
        //Add a caption to the image
        form.AddField("caption", "This is the score from the game lmao");
        // Call Graph API to upload the form
        FB.API("me/photos", HttpMethod.POST, UploadPhotoDone, form);

        if (gameHUD != null)
        {
            gameHUD.uploadPanel.SetActive(true);
            gameHUD.okayButton.gameObject.SetActive(false);
            gameHUD.uploadPanelText.text = "Uploading image of score...";
            Debug.Log("Changed text to uploading image score");
        }

        AdsManager.Instance.ShowBannerAd();

        Debug.Log("Uploading Image");
    }

    public void UploadScreenshot()
    {
        aboutToUpload = true;
        uploadSuccessful = false;

        if (FB.IsLoggedIn)
        {
            aboutToUpload = false;
            StartCoroutine(ScreenshotAndUpload());
        }
        else
        {
            GameHUD gameHUD = FindObjectOfType<GameHUD>();
            if (gameHUD != null)
            {
                gameHUD.okayButton.gameObject.SetActive(false);
                gameHUD.SetActiveUploadPanel(true);
                gameHUD.uploadPanelText.text = "Logging in...";
                Debug.Log("Changed text to logging in");
            }

            LoginFB();
        }
    }
}
