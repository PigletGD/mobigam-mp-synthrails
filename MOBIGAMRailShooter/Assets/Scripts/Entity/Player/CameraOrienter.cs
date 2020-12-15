using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrienter : MonoBehaviour
{
    private bool isLandscape = false;
    private float landscapeFOV = 60;
    private float portraitFOV;

    public Camera cam;

    private void Start()
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            portraitFOV = (Screen.height * landscapeFOV) / Screen.width;

            cam.fieldOfView = portraitFOV;

            isLandscape = false;
        }
        else
        {
            portraitFOV = (Screen.width * landscapeFOV) / Screen.height;

            isLandscape = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) && isLandscape)
        {
            cam.fieldOfView = portraitFOV;

            isLandscape = false;
        }
        else if ((Screen.orientation == ScreenOrientation.Landscape || Screen.orientation == ScreenOrientation.LandscapeRight) && !isLandscape)
        {
            cam.fieldOfView = landscapeFOV;

            isLandscape = true;
        }
    }
}
