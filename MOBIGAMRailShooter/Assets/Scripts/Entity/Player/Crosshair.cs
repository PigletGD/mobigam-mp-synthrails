using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform RT = null;
    [SerializeField] private Transform playerModel = null;
    [SerializeField] private Canvas canvas = null;

    private float distance = 0.0f;

    private Vector2 portraitImageSize = Vector2.zero;
    private Vector2 landscapeImageSize = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        distance = 15 - playerModel.position.z;

        landscapeImageSize = RT.rect.size;
        portraitImageSize = RT.rect.size * OrientationManager.Instance.uiPortraitSizeFactor;

        ChangeImageSize();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = playerModel.forward;
        rotation.Normalize();

        float factor = 15.0f / rotation.z;

        Vector2 screenPos = Camera.main.WorldToScreenPoint(playerModel.position + (playerModel.forward * factor));

        RT.anchoredPosition = screenPos - new Vector2(canvas.pixelRect.width * 0.5f, canvas.pixelRect.height * 0.5f);
    }

    public void ChangeImageSize()
    {
        if (OrientationManager.Instance.isLandscape)
            RT.sizeDelta = landscapeImageSize;
        else RT.sizeDelta = portraitImageSize;
    }
}
