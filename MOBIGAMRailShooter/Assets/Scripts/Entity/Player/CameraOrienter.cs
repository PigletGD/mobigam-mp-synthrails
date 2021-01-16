using UnityEngine;

public class CameraOrienter : MonoBehaviour
{
    private float landscapeFOV = 60;
    private float portraitFOV;

    public Camera cam;

    private void Start()
    {
        if (!OrientationManager.Instance.isLandscape)
        {
            portraitFOV = (Screen.height * landscapeFOV) / Screen.width;

            cam.fieldOfView = portraitFOV;
        }
        else portraitFOV = (Screen.width * landscapeFOV) / Screen.height;
    }

    public void ChangeFOV()
    {
        if (!OrientationManager.Instance.isLandscape) cam.fieldOfView = portraitFOV;
        else cam.fieldOfView = landscapeFOV;
    }
}