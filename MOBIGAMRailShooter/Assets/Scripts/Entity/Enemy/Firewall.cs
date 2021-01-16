using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall : MonoBehaviour
{
    [SerializeField] private RectTransform warningRT = null;

    private float portraitDistanceFromCenterVert = 0;
    private float landscapeDistanceFromCenterVert = 0;
    private float portraitDistanceFromCenterHori = 0;
    private float landscapeDistanceFromCenterHori = 0;
    private Vector3 referenceWorldPos = Vector3.zero;

    private float xLoc = 0;
    private float yLoc = 0;

    private Vector2 portraitImageSize = Vector2.zero;
    private Vector2 landscapeImageSize = Vector2.zero;

    [SerializeField] RectTransform canvas = null;
    [SerializeField] bool isVerticalWall = true;

    private void Awake()
    {
        referenceWorldPos = Camera.main.ScreenToWorldPoint(warningRT.anchoredPosition);

        landscapeImageSize = warningRT.rect.size;
        portraitImageSize = warningRT.rect.size * OrientationManager.Instance.uiPortraitSizeFactor;

        if (isVerticalWall)
        {
            landscapeDistanceFromCenterVert = warningRT.anchoredPosition.y;

            if (OrientationManager.Instance.isLandscape)
                portraitDistanceFromCenterVert = landscapeDistanceFromCenterVert * (-landscapeDistanceFromCenterVert / ((float)Screen.height * 0.5f));
            else
                portraitDistanceFromCenterVert = landscapeDistanceFromCenterVert * (-landscapeDistanceFromCenterVert / ((float)Screen.width * 0.5f));
        }
        else
        {
            landscapeDistanceFromCenterHori = warningRT.anchoredPosition.x;

            if (OrientationManager.Instance.isLandscape)
                portraitDistanceFromCenterHori = ((((landscapeDistanceFromCenterHori + ((float)Screen.width * 0.5f))) / ((float)Screen.width)) * (float)Screen.height) - ((float)Screen.height * 0.5f);
            else
                portraitDistanceFromCenterHori = ((((landscapeDistanceFromCenterHori + ((float)Screen.height * 0.5f))) / ((float)Screen.height)) * (float)Screen.width) - ((float)Screen.width * 0.5f);
        }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        Invoke("ExecuteBehaviour", 0.05f);
    }

    private void ExecuteBehaviour()
    {
        ChangeImageRectTransform();

        AudioManager.Instance.Play("Firewall");

        Invoke("DisableObject", 4.95f);
    }

    public void ChangeImageRectTransform()
    {
        Transform parentTrans = transform.parent;

        if (isVerticalWall)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(parentTrans.position.x, referenceWorldPos.y, parentTrans.position.z));

            xLoc = screenPoint.x - (float)Screen.width * 0.5f;

            if (OrientationManager.Instance.isLandscape)
                yLoc = landscapeDistanceFromCenterVert;
            else
                yLoc = portraitDistanceFromCenterVert;
        }
        else
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(referenceWorldPos.x, parentTrans.position.y, parentTrans.position.z));

            yLoc = screenPoint.y - (float)Screen.height * 0.5f;

            if (OrientationManager.Instance.isLandscape)
                xLoc = landscapeDistanceFromCenterHori;
            else
                xLoc = portraitDistanceFromCenterHori;
        }

        if (OrientationManager.Instance.isLandscape)
            warningRT.sizeDelta = landscapeImageSize;
        else
            warningRT.sizeDelta = portraitImageSize;

        warningRT.anchoredPosition = new Vector2(xLoc, yLoc);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            other.GetComponent<PlayerInfo>().TakeDamage(1);
    }

    private void DisableObject() => transform.parent.gameObject.SetActive(false);
}