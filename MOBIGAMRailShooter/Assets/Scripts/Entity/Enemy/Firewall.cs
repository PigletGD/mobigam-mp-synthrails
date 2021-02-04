using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall : MonoBehaviour
{
    public RectTransform warningRT = null;

    private float portraitDistanceFromCenterVert = 0;
    private float landscapeDistanceFromCenterVert = 0;
    private float portraitDistanceFromCenterHori = 0;
    private float landscapeDistanceFromCenterHori = 0;
    private Vector3 referenceWorldPos = Vector3.zero;

    private float xLoc = 0;
    private float yLoc = 0;

    private Vector2 portraitImageSize = Vector2.zero;
    private Vector2 landscapeImageSize = Vector2.zero;

    public RectTransform canvas = null;
    public bool isVerticalWall = true;

    public Animator animator = null;

    public Camera mainCam = null;

    private void Awake()
    {
        MeshRenderer MeshR = GetComponent<MeshRenderer>();
        MeshR.sharedMaterial = BundleManager.Instance.GetAsset<Material>("materials", "Mat_Dissolve");
        string shader = MeshR.sharedMaterial.shader.name;
        MeshR.sharedMaterial.shader = Shader.Find(shader);

        mainCam = Camera.main;
        if (mainCam != null) 
            referenceWorldPos = mainCam.ScreenToWorldPoint(warningRT.anchoredPosition);

        landscapeImageSize = warningRT.rect.size;
        portraitImageSize = warningRT.rect.size * OrientationManager.Instance.uiPortraitSizeFactor;

        animator = transform.parent.GetComponent<Animator>();

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

    private void Update()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
            if (mainCam != null)
                referenceWorldPos = mainCam.ScreenToWorldPoint(warningRT.anchoredPosition);
        }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
            if (mainCam != null)
                referenceWorldPos = mainCam.ScreenToWorldPoint(warningRT.anchoredPosition);
        }

        Invoke("ExecuteBehaviour", 0.05f);
    }

    private void ExecuteBehaviour()
    {
        if (mainCam != null)
        {
            ChangeImageRectTransform();

            if (animator == null) animator = transform.parent.GetComponent<Animator>();
        }

        AudioManager.Instance.Play("Firewall");

        Invoke("DisableObject", 4.95f);
    }

    public void ChangeImageRectTransform()
    {
        Transform parentTrans = transform.parent;

        if (isVerticalWall)
        {
            Vector2 screenPoint = mainCam.WorldToScreenPoint(new Vector3(parentTrans.position.x, referenceWorldPos.y, parentTrans.position.z));

            xLoc = screenPoint.x - (float)Screen.width * 0.5f;

            if (OrientationManager.Instance.isLandscape)
                yLoc = landscapeDistanceFromCenterVert;
            else
                yLoc = portraitDistanceFromCenterVert;
        }
        else
        {
            Vector2 screenPoint = mainCam.WorldToScreenPoint(new Vector3(referenceWorldPos.x, parentTrans.position.y, parentTrans.position.z));

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